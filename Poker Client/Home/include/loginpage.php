<div id="login">
  <div id="login-head"><a><strong>LOG-IN</strong></a></div>
  <div id="box-login">
    <div id="main-form-login">
      <div id="login-input-username">
        <input id="box-login-input-username" type="text" name="username" placeholder="Enter username here">
      </div>
      <div id="login-input-password">
        <input id="box-login-input-password" type="password" name="password" placeholder="Enter password here">
      </div>
      <div id="captcha-show-box">
        captcha show here
      </div>
      <div id="login-input-captcha">
        <input id="box-login-input-captcha" type="text" name="captcha" placeholder="Captcha code">
      </div>
      <div id="login-input-submit">
        <input type="button" name="login" value="ENTER" onclick="BoxLoginRun()">
      </div>
    </div>

  </div>
</div>
<style>
#viewer-box-back
{
  opacity: 0 !important;
}
#login
{
  width: 40%;
  height: 75%;
  position: relative;
  margin: 0 auto;
  background-color: #1b92c1;
  border-radius: 20px;
  transform: translateY(15%);
  opacity: 0.88;
  overflow: hidden;
}
#box-login
{
  position: relative;
  height: 100%;
  width: 65%;
  margin: 0 auto;
}
#login-head
{
  position: absolute;
  text-align: center;
  width: 100%;
  height: 15%;
  position: relative;
  background-color: #262626;
  color: white;
  -webkit-transition: background-color 2s;
  transition: background-color 2s;
}
#login-head a
{
  position: absolute;
  font-size: 150%;
  top: 50%;
  transform:  translate3d(-50%,-50%,0);
}
#login-input-submit, #login-input-username, #login-input-password, #login-input-captcha
{
  position: relative;
  margin: 0 auto;
  width: 100%;
  height: 10%;
  margin-top: 10%;
}
#login-input-submit input,#login-input-password input,#login-input-username input, #login-input-captcha input
{
  width: 100%;
  height: 100%;
  font-size: 110%;
  border-radius: 6px;
  color: rgb(76, 76, 76);
}
#login-input-submit input
{
  width: 102%;
  background-color: rgb(76, 164, 76);
  color : white;
}
#main-form-login
{
  height: 100%;
  width: 100%;
  position: relative;
}
#captcha-show-box
{
  position: relative;
  margin: 0 auto;
  width: 100%;
  height: 10%;
  margin-top: 10%;
  background-color: red;
}
</style>
