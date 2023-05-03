var user = {
    init: function () {
        user.registerEvents()
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var model = $(this);
            var id = model.data('id');

            $.ajax({
                url: '/Admin/Category/ChangeStatus',
                dataType: "json",
                data: { id: id },
                type: "POST",
                success: function (response) {
                    //console.log(response);
                    if (response.status == true) {
                        model.html('<i class="fa fa-check-circle"></i>');
                    } else {
                        model.html('<i class="fa fa-times-circle"></i>');
                    }
                }
            })
        })
    }
}
user.init()