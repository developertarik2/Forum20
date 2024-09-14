// Write your JavaScript code.
function CheckForm() {

   // var empty = 0;
    if ($('#titleText').val() == '') {
        document.getElementById("title").innerText = "can't be empty!!";
        return false;
    } 
    else
    {
        document.getElementById("title").innerText = "";
    }
    if ($('#contentText').val() == '') {
        document.getElementById("content").innerText = "can't be empty!!";
        return false;
        
    };
   /* $('input[type=text]').each(function () {
        preventPaste: true;
        if (this.value == "") {
            empty++;
            $("#error").show('slow');
            //return false;

        }
    })
    if (empty > 0) {
        document.getElementById("title").innerText = "can't be empty!!";
      //  alert(empty + ' empty input(s)')
        return false;
    }
    else {

    }*/
}

function checkImageType() {
    var ext = $('#photo').val().split('.').pop().toLowerCase();
    if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
        document.getElementById("imageText").innerText = "must be image Type!!";
        return false;
    }
    else
        document.getElementById("imageText").innerText = "valid format!!!";
}
$(document).ready(function () {
    $('#photo').bind('change', function () {
        var a = Math.round((this.files[0].size)/1024);
        //alert(a);
        if (a > 800) {
            document.getElementById("imageText").innerText = "file size should be less than 600kb";
          //  alert('large');
            return false;
        };
    });
});

$('#photo').change(function () {
    // above check
   var ext = $('#photo').val().split('.').pop().toLowerCase();
    if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
        document.getElementById("imageText").innerText = "must be image Type!!";
        return false;
    }
    else
        document.getElementById("imageText").innerText = "valid!!";
   /* if (this.files[0] !='') {
        var file = this.files[0];
        var fileType = file["type"];
        var validImageTypes = ["image/gif", "image/jpeg", "image/png"];
        if ($.inArray(fileType, validImageTypes) < 0) {
            // invalid file type code goes here.
            document.getElementById("imageText").innerText = "must be image Type!!";
            return false;
        }
        else
            document.getElementById("imageText").innerText = "valid!!";
    }*/
});

function CheckReply() {
    if ($('#reply').val() == '') {
        document.getElementById("replyerror").innerText = "can't be empty!!";
        return false;
    }; 
}

function Validation(evt) {
  //  var charCode = (evt.which) ? evt.which : event.keyCode; // restrict user to type only one . point in number
    var parts = evt.srcElement.value;
    if (parts.length < 0) {
        document.getElementById("title").innerText = "can't be empty!!";
        return false;
    }
    
    return true;
}

$(document).ready(function () {
    $('#addPostForm input').blur(function () {
        /*if (!$(this).val()) {
            $(this).parents('span').addClass('warning');
        }*/
        if ($(this).val().length === 0) {
            $(this).parents('p').addClass('warning');
        }
    });
});

$('#titleText').on('keyup', function () {
    if ($(this).val().length === 0)
    //$('#title').text(this.value);// or $(this).val()
    {
        document.getElementById("title").innerText = "can't be empty!!";
        return false
    }
    else 
        document.getElementById("title").innerText = "";
});

$(document).ready(function () {
    //var table =
    $('#example').DataTable({
        "oLanguage": {
            "sLengthMenu": "Display _MENU_ Replies",
        },
        "aLengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
        "pageLength": 1


    });

    $('#forumIndexTable').DataTable({
        "oLanguage": {
            "sLengthMenu": "Display _MENU_ Replies",
        },
        "aLengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
        "pageLength": 5


    });

    $('#forumList').DataTable({
        "oLanguage": {
            "sLengthMenu": "Display _MENU_ Forums",
            "sInfo": "Showing _START_ to _END_ of _TOTAL_ Forums",
            "sInfoEmpty": "Showing 0 to 0 of 0 Forums"
        },
        "aLengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
        "pageLength": 5,



    });
    $('#postList').DataTable({
        "oLanguage": {
            "sLengthMenu": "Display _MENU_ Posts",
            "sInfo": "Showing _START_ to _END_ of _TOTAL_ Posts",
            "sInfoEmpty": "Showing 0 to 0 of 0 Forums"
        },
        "aLengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
        "pageLength": 5


    });
    $('#postReplies').DataTable({
        "oLanguage": {
            "sLengthMenu": "Display _MENU_ Replies",
            "sInfo": "Showing _START_ to _END_ of _TOTAL_ Replies",
            "sInfoEmpty": "Showing 0 to 0 of 0 Replies"
        },
        "aLengthMenu": [[5, 10, 15, -1], [5, 10, 15, "All"]],
        "pageLength": 5


    });
    //table.fixedHeader.disable();
});

$(document).ready(function () {
    //var table =
    
    
});