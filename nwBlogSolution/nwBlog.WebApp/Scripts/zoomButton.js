$(window).scroll(function () {
    if ($(this).scrollTop() > 50)    // Sayfa ne kadar aşağı kayarsa buton görünsün. 100 sayısı = Kaydırma çubuğunun piksel konumu. Bu sayı değiştirilebilir.
        $("#zoomButton").fadeIn(500);    // Yukarı çık butonu ne kadar hızla ortaya çıksın. 500 milisaniye = 0,5 saniye. Bu sayı değiştirilebilir.
    else
        $("#zoomButton").fadeOut(500);    // Yukarı çık butonu ne kadar hızla ortadan kaybolsun. 500 milisaniye = 0,5 saniye. Bu sayı değiştirilebilir.
});
$(document).ready(function () {



    var varsayilanFontBuyuklugu = $(".text").css("font-size");

    $(".sifirla").click(function () {
        $(".text").css({ "font-size": varsayilanFontBuyuklugu });
    });

    $(".buyult").click(function () {
        var fontBuyukluguFloat = parseFloat($(".text").css("font-size"));
        var yeniFontBuyuklugu = fontBuyukluguFloat * 1.2;
        if (yeniFontBuyuklugu < 50)
            $(".text").css({ "font-size": yeniFontBuyuklugu });
    });

    $(".kucult").click(function () {
        var fontBuyukluguFloat = parseFloat($(".text").css("font-size"));
        var yeniFontBuyuklugu = fontBuyukluguFloat * 0.8;
        if (yeniFontBuyuklugu > 5)
            $(".text").css({ "font-size": yeniFontBuyuklugu });
    });
})