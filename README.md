# protocols

## 設定ファイル記述例

```xml:secret.config
<?xml version="1.0" encoding="utf-8"?>
<appSettings>
  <add key="unitTest:smtpHost" value="smtpサーバー"/>
  <add key="unitTest:smtpAuthUser" value="ユーザID"/>
  <add key="unitTest:smtpAuthPass" value="パスワード"/>
  <add key="unitTest:emailAddressSender" value="送信メールアドレス"/>
  <add key="unitTest:commaSeparatedEmailAddressRecipientsTo" value="受信メールアドレス1,受信メールアドレス2"/>
</appSettings>
```

