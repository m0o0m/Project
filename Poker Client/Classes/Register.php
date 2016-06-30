<?php
class Register
{
    public static function run($country,$username,$password,$passwordAgain,$email){
        if(!Validate::checkNewUsername($username)){
            echo "bad Username";
            return false;
        }
        if(!Validate::checkEmail($email)){
            echo "bad Email";
            return false;
        }
        if(!Validate::checkPass($password,$passwordAgain)) {
            echo "bad password";
            return false;
        }
        $chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        $size = strlen( $chars );
        $str = "";
        for( $i = 0; $i < 20; $i++ ) {
            $str .= $chars[ rand( 0, $size - 1 ) ];
        }
        $salt = hash("MD5",$str);

        $password = hash("MD5",$password+$salt);

        //add to database
        if(DataBase::getInstance()->set(
            "Users",
            array("country","username","password","email","salt"),
            array($country,$username,$password,$email,$salt))){

            Validate::createValidatorID($username);
            return true;
        }
        return false;
    }
}
