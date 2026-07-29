
    document.addEventListener("DOMContentLoaded", function () {

        $('#replySummernote').summernote({
            height: 250
        });

        const aiButton =
            document.getElementById("generateAIResponseButton");

        aiButton.addEventListener("click", function () {

            const messageId =
                aiButton.getAttribute("data-id");

            $.ajax({
                url: "/Message/GenerateAIResponse",
                type: "POST",
                data: {
                    messageId: messageId
                },

                success: function (response) {

                    const formattedResponse = response
                        .replace(/\r\n/g, "<br>")
                        .replace(/\n/g, "<br>");

                    const textarea =
                        document.getElementById("replySummernote");

                    const noteEditor =
                        textarea.nextElementSibling;

                    const editableArea =
                        noteEditor.querySelector(".note-editable");

                    editableArea.innerHTML = formattedResponse;
                    textarea.value = formattedResponse;
                },

                error: function (xhr) {
                    console.log(xhr.responseText);
                }
            });

        });

    });

    document.addEventListener("DOMContentLoaded", function () {

        $(document).on("click", ".mail-action", function () {

            var button = $(this);
            var icon = button.find("i");

            $.post(button.data("url"), {
                id: button.data("id"),
                __RequestVerificationToken:
                    $("#tokenForm input[name='__RequestVerificationToken']").val()
            })
            .done(function () {

                if (button.hasClass("mail-star")) {

                    if (icon.hasClass("fa-star")) {
                        icon.removeClass("fa-star starred")
                            .addClass("fa-star-o");
                    }
                    else {
                        icon.removeClass("fa-star-o")
                            .addClass("fa-star starred");
                    }
                }

                if (button.hasClass("mail-spam") ||
                    button.hasClass("mail-trash")) {

                    button.closest("tr").fadeOut(200);
                }
            })
            .fail(function (xhr) {
                console.log(xhr.status);
                console.log(xhr.responseText);
            });

        });

    });



    document.addEventListener("DOMContentLoaded", function () {

        const showReplyButton =
            document.getElementById("showReplyButton");

        const cancelReplyButton =
            document.getElementById("cancelReplyButton");

        const replyArea =
            document.getElementById("replyArea");

        showReplyButton.addEventListener("click", function () {
            replyArea.style.display = "block";

            const textarea =
                replyArea.querySelector("textarea");

            textarea.focus();
        });

        cancelReplyButton.addEventListener("click", function () {
            replyArea.style.display = "none";
        });

    });
