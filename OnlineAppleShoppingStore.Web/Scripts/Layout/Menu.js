$(document).ready(function ()
{
    var n = 0;
    $(".menu-toggle").click(function ()
    {
        $(this).siblings(".top-menu-mobile-wrapper").slideToggle("slow")
    });
    $(".top-menu.mobile .default").click(function ()
    {
        var t = $(this); $(this).siblings(".sublist").fadeIn("slow");
        n = $(".top-menu.mobile").height() + 2;
        $(".top-menu.mobile").css("height", $(t).siblings(".sublist").outerHeight()).fadeIn()
    });
    $(".top-menu-mobile-wrapper .mobile-menu-back").click(function ()
    {
        $(".top-menu.mobile .sublist:visible").length > 0 ? $(".top-menu.mobile").animate({ height: n }).find(".sublist:visible").fadeOut("slow") : $(".top-menu-mobile-wrapper").slideToggle("slow")
    });
    $(".mobile-menu-close").click(function ()
    {
        $(this).closest(".menu-mobile-wrapper").slideToggle("slow")
    })
})