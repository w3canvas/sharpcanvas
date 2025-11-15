# Maven Proxy for Claude Code Web

## Quick Start

If Maven commands are failing with `401 Unauthorized` or DNS resolution errors, use this proxy:

```bash
# 1. Start the proxy (from project root)
python3 .claude/maven-proxy.py > /tmp/maven_proxy.log 2>&1 &

# 2. Configure Maven (one-time setup)
mkdir -p ~/.m2
cat > ~/.m2/settings.xml << 'EOF'
<?xml version="1.0" encoding="UTF-8"?>
<settings xmlns="http://maven.apache.org/SETTINGS/1.0.0"
          xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
          xsi:schemaLocation="http://maven.apache.org/SETTINGS/1.0.0
                              http://maven.apache.org/xsd/settings-1.0.0.xsd">
  <proxies>
    <proxy>
      <id>local-proxy</id>
      <active>true</active>
      <protocol>http</protocol>
      <host>127.0.0.1</host>
      <port>8888</port>
      <nonProxyHosts>localhost</nonProxyHosts>
    </proxy>
  </proxies>
</settings>
EOF

# 3. Run Maven normally
mvn test
```

## When Do You Need This?

Use this proxy when you encounter:
- `401 Unauthorized` errors from Maven Central
- `Temporary failure in name resolution` for `repo.maven.apache.org`
- Maven failing to download dependencies in Claude Code Web

## How It Works

**The Problem:**
- Claude Code Web uses a proxy that requires Bearer token authentication
- Maven only supports Basic/NTLM authentication
- Result: Maven cannot download dependencies

**The Solution:**
- This Python script acts as a local proxy (localhost:8888)
- It translates Maven's standard HTTP requests into Bearer-authenticated requests
- Maven talks to localhost, the script talks to the Claude proxy

## Verification

After starting the proxy, verify it's working:

```bash
# Check if proxy is running
curl -x http://127.0.0.1:8888 -I https://repo.maven.apache.org/maven2/

# Should show: HTTP/1.1 200 OK
```

## Troubleshooting

### Proxy won't start - "Port already in use"
```bash
# Kill existing proxy
killall python3
# Or find and kill specific process
lsof -ti:8888 | xargs kill
```

### Still getting 401 errors after setup
- The JWT token expires every few hours
- Restart the proxy to pick up the new token from the environment

### Maven ignoring the proxy
- Verify ~/.m2/settings.xml exists and has the proxy configuration
- Try: `mvn dependency:resolve` to test proxy connectivity

## Advanced Usage

### Run with debug logging
```bash
python3 .claude/maven-proxy.py 2>&1 | tee /tmp/maven_proxy_debug.log &
```

### Check proxy logs
```bash
tail -f /tmp/maven_proxy.log
```

### Manually test connection through proxy
```bash
curl -x http://127.0.0.1:8888 -v https://repo.maven.apache.org/maven2/
```

## Technical Details

- **Proxy Type:** HTTP CONNECT tunnel
- **Local Port:** 8888
- **Authentication:** Extracts Bearer token from `$HTTPS_PROXY`
- **Thread Safety:** Yes (one thread per connection)
- **Dependencies:** Python 3.x stdlib only

## Credit

Based on the workaround described by Tarun Lalwani:
https://www.linkedin.com/pulse/fixing-maven-build-issues-claude-code-web-ccw-tarun-lalwani-8n7oc/
