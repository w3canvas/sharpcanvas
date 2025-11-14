#!/usr/bin/env python3
"""
Simple HTTP CONNECT proxy for NuGet that forwards to authenticated upstream proxy
"""
import socket
import select
import threading
import os
import sys
from urllib.parse import urlparse

PORT = 8888
BUFFER_SIZE = 8192

def get_upstream_proxy():
    """Extract upstream proxy details from environment"""
    proxy_url = os.environ.get('HTTPS_PROXY') or os.environ.get('HTTP_PROXY')
    if not proxy_url:
        return None

    # Parse proxy URL
    parsed = urlparse(proxy_url)
    host = parsed.hostname
    port = parsed.port or 15004

    # Extract username (contains JWT) and build auth
    if parsed.username:
        # Username format: container_NAME:jwt_TOKEN
        auth = f"{parsed.username}"
        if parsed.password:
            auth += f":{parsed.password}"
        return (host, port, auth)

    return (host, port, None)

def forward_data(source, destination):
    """Forward data between sockets"""
    try:
        while True:
            data = source.recv(BUFFER_SIZE)
            if not data:
                break
            destination.sendall(data)
    except:
        pass
    finally:
        try:
            source.close()
        except:
            pass
        try:
            destination.close()
        except:
            pass

def handle_client(client_socket, client_address):
    """Handle a client CONNECT request"""
    try:
        # Read the CONNECT request
        request = client_socket.recv(BUFFER_SIZE).decode('utf-8')
        lines = request.split('\r\n')

        if not lines[0].startswith('CONNECT'):
            client_socket.close()
            return

        # Parse CONNECT target
        parts = lines[0].split()
        if len(parts) < 2:
            client_socket.close()
            return

        target = parts[1]  # e.g., "api.nuget.org:443"
        print(f"[CONNECT] {target} from {client_address}", file=sys.stderr, flush=True)

        # Get upstream proxy
        upstream = get_upstream_proxy()
        if not upstream:
            print(f"[ERROR] No upstream proxy configured", file=sys.stderr, flush=True)
            client_socket.close()
            return

        upstream_host, upstream_port, auth = upstream

        # Connect to upstream proxy
        upstream_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        upstream_socket.settimeout(30)

        try:
            upstream_socket.connect((upstream_host, upstream_port))

            # Send CONNECT to upstream proxy with authentication
            connect_request = f"CONNECT {target} HTTP/1.1\r\n"
            connect_request += f"Host: {target}\r\n"
            if auth:
                import base64
                auth_b64 = base64.b64encode(auth.encode()).decode()
                connect_request += f"Proxy-Authorization: Basic {auth_b64}\r\n"
            connect_request += "Connection: keep-alive\r\n"
            connect_request += "\r\n"

            upstream_socket.sendall(connect_request.encode())

            # Read upstream response
            response = upstream_socket.recv(BUFFER_SIZE).decode('utf-8')

            if '200' in response.split('\r\n')[0]:
                # Success! Send 200 to client
                client_socket.sendall(b"HTTP/1.1 200 Connection Established\r\n\r\n")
                print(f"[SUCCESS] Tunnel established to {target}", file=sys.stderr, flush=True)

                # Start bi-directional forwarding
                client_to_upstream = threading.Thread(
                    target=forward_data,
                    args=(client_socket, upstream_socket)
                )
                upstream_to_client = threading.Thread(
                    target=forward_data,
                    args=(upstream_socket, client_socket)
                )

                client_to_upstream.start()
                upstream_to_client.start()

                client_to_upstream.join()
                upstream_to_client.join()
            else:
                print(f"[ERROR] Upstream proxy rejected: {response.split()[0][:50]}", file=sys.stderr, flush=True)
                client_socket.sendall(b"HTTP/1.1 502 Bad Gateway\r\n\r\n")

        except Exception as e:
            print(f"[ERROR] Upstream connection failed: {e}", file=sys.stderr, flush=True)
            client_socket.sendall(b"HTTP/1.1 502 Bad Gateway\r\n\r\n")
        finally:
            try:
                upstream_socket.close()
            except:
                pass

    except Exception as e:
        print(f"[ERROR] Client handling failed: {e}", file=sys.stderr, flush=True)
    finally:
        try:
            client_socket.close()
        except:
            pass

def main():
    upstream = get_upstream_proxy()
    if not upstream:
        print("ERROR: No HTTPS_PROXY or HTTP_PROXY environment variable found!", file=sys.stderr)
        sys.exit(1)

    print(f"Starting NuGet proxy on http://localhost:{PORT}", file=sys.stderr, flush=True)
    print(f"Upstream: {upstream[0]}:{upstream[1]}", file=sys.stderr, flush=True)
    print(file=sys.stderr, flush=True)

    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server.bind(('127.0.0.1', PORT))
    server.listen(100)

    print(f"[READY] Listening on port {PORT}...\n", file=sys.stderr, flush=True)

    try:
        while True:
            client_socket, client_address = server.accept()
            client_thread = threading.Thread(
                target=handle_client,
                args=(client_socket, client_address)
            )
            client_thread.daemon = True
            client_thread.start()
    except KeyboardInterrupt:
        print("\n[SHUTDOWN] Stopping proxy...", file=sys.stderr, flush=True)
    finally:
        server.close()

if __name__ == '__main__':
    main()
