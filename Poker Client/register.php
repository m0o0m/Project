<?php
session_start();
include_once "config.php";
if(Session::exist("user_id"))
{
  header("Location: ".$GLOBALS["game_url"]);
  die();
}else
    if(!isset($_POST["username"]) || !isset($_POST["password"]) || !isset($_POST["passwordAgain"])
        || !isset($_POST["country"])
        || !isset($_POST["email"]))
    {
      var_dump($_POST);
      die();
    }
    else
    {
        if(Register::run(
            $_POST["country"],
            $_POST["username"],
            $_POST["password"],$_POST["passwordAgain"],
            $_POST["email"]))
        {
          mkdir($GLOBALS["profiles_path"]."\\".$_POST["username"]);
          copy($GLOBALS["defaults_path"]."\\default-profile.png",$GLOBALS["profiles_path"]."\\".$_POST["username"]."\\profile.png");
          echo "Registered successfully!";
        }
    }
