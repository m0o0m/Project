<div id="login">
  <div id="box-login">
    <div id="login-head"><a><strong>LOG-IN</strong></a></div>

    <div id="main-form-login">
      <div id="login-input-username">
        <input type="text" name="username" placeholder="Enter username here">
      </div>
      <div id="login-input-password">
        <input type="password" name="password" placeholder="Enter password here">
      </div>
      <div id="captcha-show-box">
        captcha show here
      </div>
      <div id="login-input-captcha">
        <input type="text" name="captcha" placeholder="Captcha code">
      </div>
      <div id="login-input-submit">
        <input type="button" name="login">
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
  background-color: green;
  border-radius: 20px;
  transform: translateY(15%);
  opacity: 0.7;
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
  text-align: center;
  width: 100%;
  height: 10%;
  position: relative;
}
#login-head a
{
  position: absolute;
  top: 50%;
  transform: translateX(-50%);
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
}
#login-input-submit input
{
  width: 102%;
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
