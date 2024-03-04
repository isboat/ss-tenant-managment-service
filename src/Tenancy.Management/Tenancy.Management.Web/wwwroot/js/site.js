// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(".appAction").on("click", function (event) {

    // Stop form from submitting normally
    event.preventDefault();

    url = $(this).attr("href")
    message = $(this).html();
    $.ajax({
        type: "POST",
        url: url,
        data: {},
        success: function (msg) {
            Swal.fire({
                title: message + "!",
                text: "Triggered " + message + ". Reload this page to see latest!",
                icon: "success"
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Error occurred, Try again later");
        }
    });
});

$(".appMessage").on("click", function (event) {

    // Stop form from submitting normally
    event.preventDefault();

    url = $(this).attr("href")
    message = $(this).html();

    inputValue = "";
    Swal.fire({
        title: "Send Message to App",
        input: "text",
        inputLabel: "Message",
        inputValue,
        showCancelButton: true,
        inputValidator: (value) => {
            if (!value) {
                return "You need to write something!";
            }
        }
    }).then((result) => {
        if (result.value) {

            $.ajax({
                type: "POST",
                url: url + "?message=" + result.value,
                contentType: "application/json",
                datatype: "application/json",
                success: function (msg) {
                    Swal.fire({
                        title: message + "!",
                        text: "Triggered " + message + ". Reload this page to see latest!",
                        icon: "success"
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error occurred, Try again later");
                }
            });
        }
    });
});