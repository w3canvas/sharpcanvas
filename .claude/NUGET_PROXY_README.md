# NuGet Proxy for Claude Code Web

## Quick Start

If `dotnet restore` is failing with `NU1301: Unable to load the service index` errors, use this proxy:

```bash
# 1. Start the proxy (from project root)
python3 .claude/nuget-proxy.py > /tmp/nuget_proxy.log 2>&1 &

# 2. Set ALL proxy environment variables (dotnet needs all variations)
export all_proxy=http://127.0.0.1:8889
export ALL_PROXY=http://127.0.0.1:8889
export http_proxy=http://127.0.0.1:8889
export HTTP_PROXY=http://127.0.0.1:8889
export https_proxy=http://127.0.0.1:8889
export HTTPS_PROXY=http://127.0.0.1:8889

# 3. Run dotnet commands normally
dotnet restore
dotnet build
dotnet test
```

## When Do You Need This?

Use this proxy when you encounter:
- `NU1301: Unable to load the service index for source https://api.nuget.org/v3/index.json`
- NuGet package restore failures in Claude Code Web
- Connection timeouts when accessing api.nuget.org

## How It Works

**The Problem:**
- Claude Code Web uses a proxy that requires Bearer token authentication
- NuGet/dotnet doesn't support Bearer authentication
- Result: NuGet cannot download packages

**The Solution:**
- This Python script acts as a local proxy (localhost:8889)
- It translates NuGet's standard HTTP requests into Bearer-authenticated requests
- dotnet talks to localhost, the script talks to the Claude proxy

## Verification

After starting the proxy, verify it's working:

```bash
# Check if proxy is running
ps aux | grep nuget-proxy

# Test with curl
export HTTPS_PROXY=http://127.0.0.1:8889
curl -I https://api.nuget.org/v3/index.json
# Should show: HTTP/1.1 200 OK or HTTP/2 200
```

## IMPORTANT: Environment Variables

dotnet/NuGet requires **ALL** case variations of proxy environment variables to be set:
- `all_proxy` (lowercase)
- `ALL_PROXY` (uppercase)
- `http_proxy` (lowercase)
- `HTTP_PROXY` (uppercase)
- `https_proxy` (lowercase)
- `HTTPS_PROXY` (uppercase)

Simply setting `HTTPS_PROXY` alone **will not work**. You must set all six.

## Troubleshooting

### Proxy won't start - "Port already in use"
```bash
# Kill existing proxy
killall python3
# Or find and kill specific process
lsof -ti:8889 | xargs kill
```

### Still getting NU1301 errors after setup
1. Verify the proxy is running: `ps aux | grep nuget-proxy`
2. Check you've set ALL six environment variables
3. The JWT token expires every few hours - restart the proxy to pick up the new token

### dotnet ignoring the proxy
- Make sure you've set all six environment variable variations
- Verify with curl that the proxy works
- Try: `dotnet restore --verbosity detailed` to see connection details

## Advanced Usage

### Run with debug logging
```bash
python3 .claude/nuget-proxy.py 2>&1 | tee /tmp/nuget_proxy_debug.log &
```

### Check proxy logs
```bash
cat /tmp/nuget_proxy.log
```

### Helper script for environment setup
```bash
# Save this as set-proxy-env.sh
export all_proxy=http://127.0.0.1:8889
export ALL_PROXY=http://127.0.0.1:8889
export http_proxy=http://127.0.0.1:8889
export HTTP_PROXY=http://127.0.0.1:8889
export https_proxy=http://127.0.0.1:8889
export HTTPS_PROXY=http://127.0.0.1:8889

# Then source it:
# source set-proxy-env.sh
```

## Technical Details

- **Proxy Type:** HTTP CONNECT tunnel
- **Local Port:** 8889 (different from Maven proxy on 8888)
- **Authentication:** Extracts Bearer token from `$HTTPS_PROXY`
- **Thread Safety:** Yes (one thread per connection)
- **Dependencies:** Python 3.x stdlib only

## Successful Build Results (November 2025)

After setting up this proxy:
- ✅ Package restore: **SUCCESS** (all projects restored in ~6 seconds)
- ✅ Build: **SUCCESS** (2 nullable warnings only)
- ✅ Tests: **174/206 passing (84.5%)**
  - Tests.Skia.Modern: 174/206 passed
  - Tests.Skia.Standalone: 1/1 passed (100%)

### Test Failure Categories
The 32 failing tests are primarily:
- Bezier curve stroke rendering edge cases
- Complex clipping operations
- Some Path2D edge cases
- Specific transformation combinations

These are implementation details, not proxy issues. The core functionality is working!

## Credit

Based on the Maven proxy workaround, adapted for NuGet/dotnet:
https://www.linkedin.com/pulse/fixing-maven-build-issues-claude-code-web-ccw-tarun-lalwani-8n7oc/
