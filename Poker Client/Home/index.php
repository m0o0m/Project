<?php
/**
 * Created by PhpStorm.
 * User: mmdamin
 * Date: 4/28/2016
 * Time: 2:03 AM
 */
include_once "config.php";
?>
<!DOCTYPE html>
<head>
    <title>Online Poker - Golden Flop </title>
    <link href="Home/styles/styleDesktop.css" rel="stylesheet">
    <link href="Home/styles/animate.css" rel="stylesheet">
    <script src="home/scripts/globals.js"></script>
    <script src="home/scripts/jquery-1.12.2.min.js"></script>
    <script src="home/scripts/loader.js"></script>
    <script src="home/scripts/login.js"></script>
</head>
<body>
<div id="main">
<?php
  include_once "loader.php";
?>
<div id="content">
</div>
</div>
</body>
<?php
$correctArrayPage = array("home","loginpage","register","news","support");
if(isset($_GET["page"]))
{
  if(in_array($_GET["page"],$correctArrayPage))
  {
    echo '<script>loadContent("'.$_GET["page"].'");</script>';
  }
}else
{
  echo '<script>loadContent("'."home".'");</script>';

}
?>
