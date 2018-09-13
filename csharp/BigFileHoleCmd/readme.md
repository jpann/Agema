
1. Start the BigFileHoleCmd and the server that will receive the file.
2. Configure the settings (settings.json) and place in the following location:  
`C:\Users\<Your User Name>\AppData\Roaming\.BigFileHoleCmd\settings.json`
	{
		"websiteDirectory": "",
		"uploadDirectory": "",
		"bufferSizeBytes": 4096,
		"host": "localhost",
		"port": 8087
	}
3. Alternatively, you can provide these settings on the command line.
4. You may need to run: `netsh http add urlacl url=http://*:<port>/ user=<username>` with elevated privs on the server.

5. Then on the client, download curl: https://curl.haxx.se/download.html

6. Post the File with curl. Examples:

curl --tr-encoding -X POST -v -T <Path To File> http://localhost:8087


