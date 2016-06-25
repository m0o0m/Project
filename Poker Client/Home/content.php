<header>
    <div id="site-logo"></div>
    <?php if(!isset($_POST["page"]) || ($_POST["page"] != "loginpage" && $_POST["page"] != "register") ){ ?>
    <div id="head-login-box">
        <div id="input-username"><input id="password" placeholder=" username" class="headinput" type="text" name="username"></div>
        <div id="input-password"><input id="username" placeholder=" password" class="headinput" type="password" name="password"></div>
        <div id="login-btn"><a>Login</a></div>
        <div id="register-top-btn"><a>Register</a></div>
        <div id="back-shadow"></div>
    </div><?php } ?>
</header>
<div id="circles-container">
<div id="circles">
    <div class="circle" id="home-btn"><a>HOME</a></div>
    <div class="circle" id="news-btn"><a>NEWS</a></div>
    <div class="circle" id="loginpage-btn"><a>LOGIN</a></div>
    <div class="circle" id="register-btn"><a>REGISTER</a></div>
    <div class="circle" id="support-btn"><a>SUPPORT</a></div>
</div>
</div>
<div id="home">
  <div id="viewer" >
    <div id="viewer-box-back"></div>
    <div class="viewer-box" id="viewer-box1">
      <?php
      if(isset($_POST["page"]))
      {
        switch($_POST["page"])
        {
          case "loginpage":
            include_once "include/loginpage.php";
            break;
          case "register":
            include_once "include/register.php";
            break;
          case "news":
            include_once "include/news.php";
            break;
          case "about":
            include_once "include/news.php";
            break;
          case "support":
            include_once "include/news.php";
            break;
          default:
            include_once "include/home.php";
        }
      }
      else{
        include_once "include/home.php";
      }
      ?>
    </div>
  </div>
</div>
