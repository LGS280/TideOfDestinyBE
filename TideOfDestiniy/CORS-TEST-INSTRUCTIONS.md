# How to Test CORS Configuration

## Method 1: Browser DevTools (Easiest - Recommended)

1. **Open your frontend** at `https://tide-of-destiny-client.vercel.app` in Chrome/Edge/Firefox

2. **Open DevTools**:
   - Press `F12` or `Ctrl+Shift+I` (Windows) / `Cmd+Option+I` (Mac)
   - Go to the **Network** tab

3. **Make a request**:
   - Try to load data that calls your API
   - Or go to Console tab and run:
     ```javascript
     fetch('https://tideofdestinybe.onrender.com/api/News?category=1', {
       method: 'GET',
       headers: { 'Content-Type': 'application/json' }
     })
     .then(r => r.json())
     .then(data => console.log('✅ Success:', data))
     .catch(err => console.error('❌ Error:', err))
     ```

4. **Check the request**:
   - In Network tab, click on the failed request
   - Check the **Headers** tab:
     - **Request Headers**: Look for `Origin: https://tide-of-destiny-client.vercel.app`
     - **Response Headers**: Look for:
       - `Access-Control-Allow-Origin: https://tide-of-destiny-client.vercel.app` ✅
       - `Access-Control-Allow-Methods: *` or specific methods ✅
       - `Access-Control-Allow-Headers: *` or specific headers ✅

5. **Check for OPTIONS request**:
   - Before your actual request, you should see an OPTIONS request (preflight)
   - If OPTIONS succeeds (200), CORS preflight is working ✅
   - If OPTIONS fails or is missing, CORS is not configured ❌

## Method 2: HTML Test Tool

1. **Open `test-cors.html`** in your browser
   - Double-click the file or open it from your file explorer
   - Or serve it from a local web server

2. **Enter your API details**:
   - API URL: `https://tideofdestinybe.onrender.com`
   - Endpoint: `/api/News?category=1` (or any public endpoint)

3. **Run the tests**:
   - Click "Run GET Test" - Should show ✅ if CORS works
   - Click "Run OPTIONS Test" - Tests preflight requests
   - Click "Check Headers" - Shows all CORS headers

## Method 3: PowerShell Script (Windows)

1. **Open PowerShell** in your project directory

2. **Run the script**:
   ```powershell
   .\test-cors.ps1
   ```

3. **Or with custom parameters**:
   ```powershell
   .\test-cors.ps1 -ApiUrl "https://tideofdestinybe.onrender.com" -Endpoint "/api/News?category=1" -Origin "https://tide-of-destiny-client.vercel.app"
   ```

4. **Check the output**:
   - ✅ Green = CORS is working
   - ❌ Red = CORS is NOT working

## Method 4: curl Command (Command Line)

**Test OPTIONS (Preflight)**:
```bash
curl -X OPTIONS "https://tideofdestinybe.onrender.com/api/News?category=1" \
  -H "Origin: https://tide-of-destiny-client.vercel.app" \
  -H "Access-Control-Request-Method: GET" \
  -H "Access-Control-Request-Headers: Content-Type" \
  -v
```

**Test GET Request**:
```bash
curl -X GET "https://tideofdestinybe.onrender.com/api/News?category=1" \
  -H "Origin: https://tide-of-destiny-client.vercel.app" \
  -v
```

Look for these headers in the response:
- `Access-Control-Allow-Origin: https://tide-of-destiny-client.vercel.app`
- `Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS` (or `*`)
- `Access-Control-Allow-Headers: *` (or specific headers)

## What to Look For

### ✅ CORS is Working If:
- Response includes `Access-Control-Allow-Origin` header
- The header value matches your origin (`https://tide-of-destiny-client.vercel.app`) or is `*`
- OPTIONS preflight requests return 200 OK
- Actual requests succeed without CORS errors in console

### ❌ CORS is NOT Working If:
- No `Access-Control-Allow-Origin` header in response
- Browser console shows "CORS policy" errors
- OPTIONS preflight returns error or missing
- Network tab shows failed requests with CORS errors

## Quick Browser Console Test

Open browser console on your Vercel site and run:

```javascript
// Quick test
fetch('https://tideofdestinybe.onrender.com/api/News?category=1')
  .then(response => {
    console.log('✅ CORS Headers:', {
      'Access-Control-Allow-Origin': response.headers.get('Access-Control-Allow-Origin'),
      'Access-Control-Allow-Methods': response.headers.get('Access-Control-Allow-Methods'),
      'Access-Control-Allow-Headers': response.headers.get('Access-Control-Allow-Headers')
    });
    return response.json();
  })
  .then(data => console.log('✅ Success!', data))
  .catch(error => console.error('❌ CORS Error:', error));
```

If you see the data, CORS is working! ✅

