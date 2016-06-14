<?php
session_start();
include_once "config.php";
if(Session::exist("user_id"))
{
    header("Location: ".$GLOBALS["game_url"]);
    die();
}
if(isset($_POST["password"]) && isset($_POST["username"])) {
    if(Validate::login($_POST["password"],$_POST["username"]))
    {
        Session::set("user_id",$_POST["username"]);
        header("Location: ".$GLOBALS["game_url"]);
    }
    else
        echo "Username or Password is incorrect!</br></br>";
}
if(!Session::exist("user_id"))
{ echo "<h1>Login</h1>";?>
<form id="login-form" method="post">
    </br></br>
    <input type="text" name="username">
    </br></br>
    <input type="password" name="password">
    </br></br>
    <input type="submit">
    </br></br>
</form>
<?php
}
?>