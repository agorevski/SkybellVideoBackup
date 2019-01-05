# SkyBellVideoBackup
Small tool that downloads your SkyBell HD videos so you don't have to pay the monthly fee.

Usage:  BackupVideos.exe --o "C:\saveFolder" --u myusername@gmail.com --p mypassword

*  o - The base output directory to save the arlo files
*  u - Your email address
*  p - Your password

You can run this tool serially indefinitely - it will find (and skip) pre-downloaded files.

# Decompiling the APK

* Turn APK into Java (I used JADX)
* Used code analysis to determine API endpoints / types of requests
* Search clues  (findstr /snip /C:"something" *.java):  @GET, @POST, @PATCH, @DELETE, @PUT, @FormUrlEncoded, @Headers, @Field, \"x- (special headers), application/json, endpoints, http://, https://, clientId, clientSecret, access_token
* Craft responses in Fiddler to see what happens