function BoxLoginRun()
{
  var passwordElem = document.getElementById("box-login-input-password");
  var usernameElem = document.getElementById("box-login-input-username");
  var captchaElem  = document.getElementById("box-login-input-captcha");

  if(passwordElem != null && usernameElem != null && captchaElem != null ){
    if(passwordElem.value != null && usernameElem.value != null && captchaElem.value != null)
    {
      $("#login-head").css("color","white");
      $("#login-head a").text("PLEASE WAIT...");
      $("#login-head").css("background-color","#262626");
      setTimeout(function()
      {
          $.post(
          "login.php",
            {
              password: passwordElem.value,
              username: usernameElem.value,
              captcha : captchaElem.value
            },
            function(data,status)
            {
              if(status == "success")
              {
                console.log(data);
                if(data == "success")
                {
                  $("#login-head").css("color","white");
                  $("#login-head a").text(data);
                  $("#login-head").css("background-color","#4ca64c");
                  setTimeout(function()
                  {
                    
                  },2000);
                }
                else
                {
                  $("#login-head").css("color","white");
                  $("#login-head a").text(data);
                  $("#login-head").css("background-color","#e52951");
                }
              }
            }
          );
      },2000);
    }
  }
}

function register()
{
  var usernameElem = document.getElementById("input-register-username");
  var passwordElem = document.getElementById("input-register-password");
  var passwordAgainElem = document.getElementById("input-register-passwordAgain");
  var countryElem = document.getElementById("input-register-country");
  var emailElem = document.getElementById("input-register-email");
  var captchaElem = document.getElementById("input-register-captcha");

  if(usernameElem!=null && passwordElem != null && passwordAgainElem != null &&
      countryElem != null && captchaElem!= null ){
    setTimeout(function(){
      $.post("register.php",
        {
          cache: false,
          username: usernameElem.value,
          password: passwordElem.value,
          passwordAgain: passwordAgainElem.value,
          email: emailElem.value,
          captcha: captchaElem.value,
          country: countryElem.value
        },
        function(data,status)
        {
          alert(data);
        }
      );
    },2000);
  }
}
