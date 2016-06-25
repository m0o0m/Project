<?php
session_start();
include_once "config.php";
if(Session::exist("user_id"))
{
  echo "done!";
  header("Location: ".$GLOBALS["game_url"]);
  die();
}
if(isset($_POST["password"]) && isset($_POST["username"])) {
    if(Validate::login($_POST["password"],$_POST["username"]))
    {
        Session::set("user_id",$_POST["username"]);
        header("Location: ".$GLOBALS["game_url"]);
        die();
    }
    else
    {
        echo "Username or Password is incorrect!";
        die();
    }
}
?>
