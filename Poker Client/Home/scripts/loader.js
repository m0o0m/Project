/**
 * Created by mmdamin on 4/28/2016.
 */
function playLoader()
{
    var cards =  $(".card");
    var i = 0;
    var flag = 0;
    setInterval(function(){
        if(flag == 0 && i < 5 ) {
            $(cards[i++]).css("background-color","#ffd700");
        }
        else if(flag == 1 && i > -1)
        {
            $(cards[i--]).css("background-color","white");
        }
        else
        {
            if(i == 5){
                flag = 1;
                i = 4;
            }
            if(i == -1){
                flag = 0;
                i = 0;
            }
        }
    },60);
}
function hideLoader()
{
    $("#loader").removeClass("animated");
    $("#loader").removeClass("bounceOutDown");
    $("#loader").removeClass("bounceOutUp");
    $(".card").removeClass("flip");
    $(".card").removeClass("animated");
    $("#loader").addClass("bounceOutDown");
    $("#loader").addClass("animated");
    setTimeout(function(){$("#loader").hide()},1000);
}
function showLoader()
{
    $("#loader").show()
    $("#loader").removeClass("animated");
    $("#loader").removeClass("bounceInUp");
    $("#loader").removeClass("bounceOutDown");
    $("#loader").addClass("bounceInUp");
    $("#loader").addClass("animated");
    setTimeout(function(){
        $(".card").addClass("flip");
        $(".card").addClass("animated");
    },400);
}
function installCirclesEvents()
{
    $(".circle").mouseover(function(){
        $(this).removeClass("animated");
        $(this).removeClass("jello");
        var t = this;
        setTimeout(function(){
            $(t).addClass("animated");
            $(t).addClass("jello");
        },100);
    });
    $(".circle").click(function(){
        showLoader();
        var t = this;
        setTimeout(function(){
            hideLoader();
        },1500);
    });
}
function loadHome()
{
    $(".circle").removeClass("enable-btn");
    $("#home-btn").addClass("enable-btn");
}
function loadAbout()
{
    $(".circle").removeClass("enable-btn");
    $("#about-btn").addClass("enable-btn");
}
$(document).ready(function(){
    playLoader();
    setTimeout(function(){
        loadHome();
        hideLoader();
    },1);
    installCirclesEvents();
});