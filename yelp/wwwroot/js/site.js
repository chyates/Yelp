// Write your Javascript code.

// Code for the login registration home toggle page
$(document).ready(function () {
    $('#login-form-link').click(function (e) {
        $("#login-form").delay(100).fadeIn(100);
        $("#register-form").fadeOut(100);
        $('#register-form-link').removeClass('active');
        $(this).addClass('active');
        e.preventDefault();
    });
    $('#register-form-link').click(function (e) {
        $("#register-form").delay(100).fadeIn(100);
        $("#login-form").fadeOut(100);
        $('#login-form-link').removeClass('active');
        $(this).addClass('active');
        e.preventDefault();
    });

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#upload_img').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#fileup input").click(function() {
        this.value = "";
        $("#files").children().remove();
        $('#upload_img').attr("src", "");
    });
    $("#fileup input").change(function() {
        var filename = this.value;
        var ext = filename.split('.').pop().toLowerCase();
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg', 'bmp']) == -1) {
            alert('You have selected an invalid extension!');
            this.value = "";
            $("#files").children().remove();
            $('#upload_img').attr("src", "");
            return;
        }
        filename_new = filename.replace("C:\u005cfakepath", "");
        $("#files").children().remove();
        $("<p class='mt-2'><span class='font-weight-bold'>Selected File:</span>  " + filename_new + "</p>").appendTo('#files');
        readURL(this);
    });

    $("#multiSelect").click(function () {
        var multiSelect = $.map($("#multiSelect option:selected"), function (el, i) {
            return $(el).text();
        });
        $("#result").val(multiSelect.join(", "));
    });
});