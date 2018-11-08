// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
'use strict'
window.onload = function () {
    console.log("On load page");
    let isVisible = false;
    window.onscroll = function (e) {

        console.log("On scroll");
        if (document.documentElement.scrollTop < 128) {
            if (window.innerWidth > 780) {
                $("#cat-nav").css({
                    "position": "relative",
                    "top": ""
                });
            }
            if (isVisible) {
                $("#btn-up").fadeOut();
                isVisible = false;
            }


        }
        else {
            if (!isVisible) {
                $("#btn-up").fadeIn();
                isVisible = true;
            }
            if (window.innerWidth > 780) {
                $("#cat-nav").css({
                    "position": "fixed",
                    "top": "63px"
                });
            }
            
        }
        console.log(document.documentElement.scrollTop);
    }
    $("#btn-up").click(function (event) {
        $.scrollTo("#scroll-me", 1000, {
            onAfter: function () {
                //alert("Hi");
            }
        });
    });
    //home/index option button
    $(".option-button").on("click", function () {
        $("#options").fadeToggle();
    });

    $("#more").click((e) => {
        $("#more").text("Load");
        $.ajax({
            dataType: 'json',
            url: "/Home/GetAuthors",
            success: (data) => {
                console.log(typeof data);
                $("#authors ul").html("");
                
                for (let obj of data) {
                    console.log(obj);
                    $("#authors ul").append("<li><a href='/Home/Index?categoryId=&authorId=" + obj.id + "'>" + obj.name + "</a></li>");
                }
                sessionStorage.setItem("authors", JSON.stringify(data));
            }
        }
        );
    });

    if (sessionStorage.getItem("authors")) {
        $("#authors ul").html("");
        let authors = sessionStorage.getItem("authors");
        console.log(authors);
        authors = JSON.parse(authors);
        for (let obj of authors) {
            console.log(obj);
            $("#authors ul").append("<li><a href='/Home/Index?categoryId=&authorId=" + obj.id + "'>" + obj.name + "</a></li>");
        }
    }
}