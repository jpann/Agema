
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

4. Then on the client, download curl

5. Post the File with curl. Examples:

curl --tr-encoding -X POST -v -T \\nas-1\backups\dcr\bmp-tracking\BMPTracking_backup_2018_09_03_173444_3470964.bak http://localhost:8087
curl --tr-encoding -X POST -v -T  W:\ESRI\PostgreSQL_DBMS_for_Windows_922_136133.exe  http://localhost:8087

curl --tr-encoding -X POST -v -T  W:\ESRI\PostgreSQL_DBMS_for_Windows_922_136133.exe  http://bigfilehole.worldviewsolutions.net 


netsh http add urlacl url=http://*:80/ user=WVS\Jay

bigfilehole.worldviewsolutions.net

netsh http add urlacl url=http://bigfilehole.worldviewsolutions.net:80/ user=WVS\Jay


bigfilehole.worldviewsolutions.net