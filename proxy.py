#!/usr/bin/env python3
"""
Simple HTTP/HTTPS proxy to work around authenticated proxy issues.
This proxy listens locally and forwards requests directly, bypassing proxy auth.
"""

import http.server
import socketserver
import urllib.request
import urllib.error
import ssl
import sys
from urllib.parse import urlparse

PORT = 8888

class ProxyHandler(http.server.SimpleHTTPRequestHandler):
    def do_GET(self):
        self.proxy_request()

    def do_POST(self):
        self.proxy_request()

    def do_HEAD(self):
        self.proxy_request()

    def do_PUT(self):
        self.proxy_request()

    def proxy_request(self):
        try:
            # Get the target URL
            url = self.path
            if not url.startswith('http'):
                url = f"https://{self.headers['Host']}{self.path}"

            print(f"[PROXY] {self.command} {url}", file=sys.stderr)

            # Create request with headers
            headers = {}
            for key, value in self.headers.items():
                if key.lower() not in ['host', 'connection', 'proxy-connection']:
                    headers[key] = value

            # Read body if present
            content_length = self.headers.get('Content-Length')
            body = None
            if content_length:
                body = self.rfile.read(int(content_length))

            # Create request
            req = urllib.request.Request(url, data=body, headers=headers, method=self.command)

            # Create SSL context that doesn't verify certificates (for corporate proxies)
            ctx = ssl.create_default_context()
            ctx.check_hostname = False
            ctx.verify_mode = ssl.CERT_NONE

            # Make the request bypassing the proxy
            try:
                with urllib.request.urlopen(req, context=ctx, timeout=30) as response:
                    # Send response status
                    self.send_response(response.status)

                    # Send response headers
                    for key, value in response.headers.items():
                        if key.lower() not in ['connection', 'transfer-encoding']:
                            self.send_header(key, value)
                    self.end_headers()

                    # Send response body
                    self.wfile.write(response.read())

                    print(f"[PROXY] ✓ {response.status} {url}", file=sys.stderr)
            except urllib.error.HTTPError as e:
                # Forward HTTP errors
                self.send_response(e.code)
                for key, value in e.headers.items():
                    if key.lower() not in ['connection', 'transfer-encoding']:
                        self.send_header(key, value)
                self.end_headers()
                if e.fp:
                    self.wfile.write(e.fp.read())
                print(f"[PROXY] ✗ {e.code} {url}", file=sys.stderr)

        except Exception as e:
            print(f"[PROXY] ERROR: {e}", file=sys.stderr)
            try:
                self.send_error(502, f"Proxy Error: {str(e)}")
            except:
                pass

    def log_message(self, format, *args):
        # Suppress default logging
        pass

if __name__ == '__main__':
    print(f"Starting proxy server on http://localhost:{PORT}")
    print("Configure NuGet to use this proxy:")
    print(f"  export http_proxy=http://localhost:{PORT}")
    print(f"  export https_proxy=http://localhost:{PORT}")
    print()

    with socketserver.TCPServer(("127.0.0.1", PORT), ProxyHandler) as httpd:
        try:
            httpd.serve_forever()
        except KeyboardInterrupt:
            print("\nShutting down proxy server...")
            sys.exit(0)
