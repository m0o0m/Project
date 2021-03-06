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
        console.log(this.id.replace("-btn","").trim());
        loadContent(this.id.replace("-btn","").trim());
    });
}
function fixer()
{
  $(".circle a").css("font-size", $(".circle").width()/5 + "px");
  $("#login-btn a").css("font-size", $("#login-btn").width()/5 + "px");
  $("#register-top-btn a").css("font-size", $("#register-top-btn").width()/10 + "px");
  $("#viewer-box-back").css("top",$(".viewer-box").offsetTop + "px");
  $("#viewer-box-back").css("left",$(".viewer-box").offset().left + "px");
}
function loadContent(name)
{
  setTimeout(function(){
    contentContainer = document.getElementById("content");
    if(contentContainer != null){
      $.post("/home/content.php",
      {
        page : name,
        cache: false
      },
      function(data,status){
        contentContainer.innerHTML = data;
        setTimeout(function(){
        installCirclesEvents();
        $(".circle").css("box-shadow","1px 1px 50px 15px black")
        $("#"+name+"-btn").css("box-shadow","1px 1px 50px 15px yellow");
        fixer();
        hideLoader();},1000);
      });
    }
  },1000);
}
$(document).ready(function(){
    playLoader();
    window.addEventListener("resize",function(){fixer();});
});
