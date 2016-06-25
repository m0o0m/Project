<div id="register-container">
  <div id="register-container-head">REGISTER</div>
  <hr>
  <form id="register-form" method="post" action="">
    <div class="input-holder"><input type="text" name="username" placeholder="Enter username "></div>
    <div class="input-holder"><input type="text" name="email" placeholder="Enter email (example : name@mailService.com) "></div>
    <div class="input-holder"><input id="input-register-password" type="password" name="password" placeholder="Enter password :"><input id="input-register-passwordAgain" type="password-again" name="password" placeholder="Enter password again :"></div>
    <div id="captcha-box"><div id="captcha"></div><div id="captcha-refresh"></div></div>
    <div class="input-holder"><input type="text" name="captcha" placeholder="Enter captcha code"></div>
    <div class="input-holder"><input type="submit" name="submit"></div>
    <div class="input-holder"><input type="button" name="reset" value="reset"></div>
  </form>
</div>
<style>
#register-form
{
  height: 100%;
  width: 80%;
  display: block;
  margin: 0 auto;
}
#viewer-box-back
{
  opacity: 0 !important;
}
#register-container-head
{
  text-align: center;
  width: 100% !important;
  color: white;
  font-size: 160%;
}
#register-container
{
  position: absolute;
  background-color: green;
  opacity: 0.8;
  width: 50%;
  height: 80%;
  top: 50%;
  left : 50%;
  transform: translate3d(-50%,-50%,0);
  border-radius: 6px;
}
.input-holder
{
  width: 100%;
  height: 7.3%;
  position: relative;
  margin-top : 5.2%;
}
input
{
  height: 100%;
  display: block;
  position: relative;
  width : 100%;
  margin:0 auto;
}
#input-register-passwordAgain
{
  float: left;
  width: 45%;
  margin-left: 1%;
}
#input-register-password
{
  float: left;
  width: 45%;
}
#captcha-box
{
  position: relative;
  width: 100%;
  height: 12%;
  margin-top : 5.2%;
  background-color: silver;
}
#captcha
{
  position: absolute;
  height: 100%;
  width: 80%;
  left: 0;
}
#captcha-refresh
{
  position: absolute;
  width: 10%;
  height : 100%;
  background-color: #556b2f;
  right: 0;
}
</style>
