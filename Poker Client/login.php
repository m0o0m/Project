<?php
session_start();
include_once "config.php";
if(Session::exist("user_id"))
{
  echo "success";
  die();
}
if(isset($_POST["password"]) && isset($_POST["username"])) {
    if(Validate::login($_POST["password"],$_POST["username"]))
    {
        Session::set("user_id",$_POST["username"]);
        echo "success";
        die();
    }
    else
    {
        echo "incorrect data";
        die();
    }
}
?>
