
1. Start the BigFileHoleCmd and the server that will receive the file.
2. Configure the UploadDirectory setting:
  <applicationSettings>
    <BigFileHoleCmd.Properties.Settings>
      <setting name="UploadDirectory" serializeAs="String">
        <value>f:\tmp</value>
      </setting>
    </BigFileHoleCmd.Properties.Settings>
  </applicationSettings>

3. You may need to run: `netsh http add urlacl url=http://*:<port>/ user=<username>` with elevated privs on the server.

4. Then on the client, download curl: https://curl.haxx.se/download.html

5. Post the File with curl. Examples:

curl --tr-encoding -X POST -v -T <Path To File> http://localhost:8087
