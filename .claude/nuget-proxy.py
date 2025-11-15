#!/usr/bin/env python3
"""
NuGet Proxy for Claude Code Web

This proxy solves a NuGet networking issue in Claude Code Web (CCW) where NuGet cannot
directly connect to NuGet.org due to Bearer token authentication requirements.

PROBLEM:
- The Claude Code proxy requires Bearer token authentication
- NuGet doesn't support Bearer authentication in its proxy configuration
- This causes "NU1301: Unable to load the service index" errors

SOLUTION:
This script creates a local HTTP proxy that:
1. Listens on localhost:8889 (different from Maven proxy on 8888)
2. Accepts standard HTTP/HTTPS requests from NuGet
3. Forwards them to the Claude Code proxy with proper Bearer token authentication

USAGE:
1. Ensure HTTPS_PROXY environment variable is set (automatic in CCW)
2. Run this script in the background:
   python3 .claude/nuget-proxy.py > /tmp/nuget_proxy.log 2>&1 &

3. Configure NuGet to use the local proxy:
   export HTTP_PROXY=http://127.0.0.1:8889
   export HTTPS_PROXY=http://127.0.0.1:8889

4. Run dotnet commands normally:
   dotnet restore
   dotnet build
   dotnet test

TECHNICAL DETAILS:
- Extracts JWT token from HTTPS_PROXY environment variable
- Strips "jwt_" prefix from the token (required by the upstream proxy)
- Establishes HTTP CONNECT tunnels with Bearer authorization
- Provides bidirectional data forwarding once tunnel is established

REQUIREMENTS:
- Python 3.x
- HTTPS_PROXY environment variable (automatically set in CCW)

TROUBLESHOOTING:
- If you get "invalid_token" errors, the JWT may have expired. Restart the session.
- Check /tmp/nuget_proxy.log for detailed proxy activity
- Ensure no other process is using port 8889

CREDIT:
Based on the Maven proxy workaround for CCW, adapted for NuGet.
"""

import socket
import threading
import os
import sys
from urllib.parse import urlparse

# Parse the upstream proxy from environment
HTTPS_PROXY = os.environ.get('HTTPS_PROXY', '')
if not HTTPS_PROXY:
    print("Error: HTTPS_PROXY environment variable not set")
    print("This script requires Claude Code Web environment")
    sys.exit(1)

# Parse proxy URL: http://user:jwt_token@host:port
parsed = urlparse(HTTPS_PROXY)
UPSTREAM_HOST = parsed.hostname
UPSTREAM_PORT = parsed.port

# The JWT token is in the password field, starting with "jwt_" - we need to strip that prefix
JWT_TOKEN = parsed.password if parsed.password else ''
if JWT_TOKEN.startswith('jwt_'):
    JWT_TOKEN = JWT_TOKEN[4:]  # Remove "jwt_" prefix

if not JWT_TOKEN:
    print("Error: Could not extract JWT token from HTTPS_PROXY")
    sys.exit(1)

print(f"Starting NuGet proxy on localhost:8889")
print(f"Forwarding to: {UPSTREAM_HOST}:{UPSTREAM_PORT}")
print(f"JWT token extracted ({len(JWT_TOKEN)} chars)")

LOCAL_HOST = '127.0.0.1'
LOCAL_PORT = 8889

def handle_client(client_socket):
    """Handle a client connection from NuGet/dotnet"""
    try:
        # Read the request from NuGet
        request = client_socket.recv(4096).decode('utf-8', errors='ignore')
        lines = request.split('\r\n')

        if not lines[0]:
            client_socket.close()
            return

        # Handle CONNECT requests (HTTPS tunneling)
        if lines[0].startswith('CONNECT'):
            parts = lines[0].split()
            if len(parts) < 2:
                client_socket.close()
                return

            target = parts[1]

            # Connect to upstream Claude Code proxy
            upstream = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            upstream.connect((UPSTREAM_HOST, UPSTREAM_PORT))

            # Send CONNECT request with Bearer authentication (WITHOUT "jwt_" prefix)
            connect_request = f"CONNECT {target} HTTP/1.1\r\n"
            connect_request += f"Host: {target}\r\n"
            connect_request += f"Proxy-Authorization: Bearer {JWT_TOKEN}\r\n"
            connect_request += "Proxy-Connection: Keep-Alive\r\n"
            connect_request += "\r\n"

            upstream.sendall(connect_request.encode('utf-8'))

            # Read response from upstream proxy
            response = upstream.recv(4096)
            client_socket.sendall(response)

            # If tunnel established (HTTP 200), set up bidirectional forwarding
            if b'200' in response:
                def forward(source, destination):
                    try:
                        while True:
                            data = source.recv(4096)
                            if not data:
                                break
                            destination.sendall(data)
                    except:
                        pass
                    finally:
                        source.close()
                        destination.close()

                # Start forwarding threads
                c2u = threading.Thread(target=forward, args=(client_socket, upstream))
                u2c = threading.Thread(target=forward, args=(upstream, client_socket))
                c2u.daemon = True
                u2c.daemon = True
                c2u.start()
                u2c.start()
                c2u.join()
                u2c.join()
            else:
                client_socket.close()
                upstream.close()
        else:
            # Handle regular HTTP requests (non-CONNECT)
            # This shouldn't happen for HTTPS, but handle it just in case
            client_socket.close()

    except Exception as e:
        print(f"Error handling connection: {e}", file=sys.stderr)
        try:
            client_socket.close()
        except:
            pass

def main():
    """Main proxy server loop"""
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)

    try:
        server.bind((LOCAL_HOST, LOCAL_PORT))
    except OSError as e:
        print(f"Error: Could not bind to {LOCAL_HOST}:{LOCAL_PORT}")
        print(f"Port may already be in use. Error: {e}")
        sys.exit(1)

    server.listen(10)

    print(f"Proxy listening on {LOCAL_HOST}:{LOCAL_PORT}")
    print("Ready to forward NuGet/dotnet requests")
    print("Press Ctrl+C to stop\n")

    try:
        while True:
            client, addr = server.accept()
            thread = threading.Thread(target=handle_client, args=(client,))
            thread.daemon = True
            thread.start()
    except KeyboardInterrupt:
        print("\nShutting down proxy...")
        server.close()

if __name__ == '__main__':
    main()
