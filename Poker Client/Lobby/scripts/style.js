/**
 * Created by mmdamin on 3/24/2016.
 */

function fixStyle(){
    if( $(window).height() < $(window).width()){
        $("body").width($(window).height()*1.8);
        $("body").height($(window).height());
        if( ($(window).width() - $("body").width()) > 0)
            $("body").css("left",( ( $(window).width() - $("body").width())/2) + "px");
        $("body").css("top","0px" );
    }
    else if( $(window).height() >= $(window).width()){
        $("body").width($(window).width());
        $("body").height($(window).width()/1.8);
        if( (($(window).height() - $("body").height())/2) > 0)
            $("body").css("top",(( ($(window).height()) - $("body").height())/2) + "px" );
        $("body").css("left","0px" );
    }
    $(".tablehandle-info").css("font-size",$(".cashtable-infobox").height() * 0.5);
    $(".sidebar-btn").css("font-size",$(".sidebar-btn").width() * 0.15 + "px");
    $("#infobox-container-header").css("font-size",$("#infobox-container-header").height() * 0.6);
    $(".profile-bottom-btn").css("font-size",$(".profile-bottom-btn").height() * 0.6);
}