
function alertDialog(settings) {  
    Swal.fire({
        type: settings.type,
        title: settings.title,
        text: settings.text
    });
}
function confirmDialog(settings) {
    const swalWithBootstrapButtons = Swal.mixin({
        buttonsStyling: true
    });
    let confirmButtonText = 'Confirmer';
    let cancelButtonText = 'Annuler';
    if (settings.confirmButtonText) {
        confirmButtonText = settings.confirmButtonText;
    }
    if (settings.cancelButtonText) {
        cancelButtonText = settings.cancelButtonText;
    }
    swalWithBootstrapButtons.fire({
        title: settings.title,
        text: settings.text,
        type: settings.type,
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText,
        allowOutsideClick: false

    }).then(function(result){
        if (result.value) {
            if ($.isFunction(settings.confirmCallback.actionCallback)) {
                settings.confirmCallback.actionCallback();
            }
                    
            if (settings.confirmCallback.notification.canShow) {
                swalWithBootstrapButtons.fire(
                    settings.confirmCallback.notification.title,
                    settings.confirmCallback.notification.text,
                    'success'
                );
            }

        } else if (

            result.dismiss === Swal.DismissReason.cancel
        ) {
            if ($.isFunction(settings.cancelCallback.actionCallback)) {
                settings.cancelCallback.actionCallback();
            }
            if (settings.cancelCallback.notification.canShow) {

                swalWithBootstrapButtons.fire(
                    settings.cancelCallback.notification.title,
                    settings.cancelCallback.notification.text,
                    'error'
                );
            }

        }
    });
}
