$(document).ready(function () {

    ObsIsReadOnly();
});





function ObsIsReadOnly() {
        $("#ObsvInfoGen").addClass('readonly');
        $("#ObsvCotisation").addClass('readonly');
        $("#ObsvEngagement").addClass('readonly');
        $("#ObsvMntRef").addClass('readonly');
        $("#ObsvRefGest").addClass('readonly');
        /*$("#btnEnregistrerVisualisationObservations").attr('style', 'visibility:hidden');*/
        $("#ObsvInfoGen").attr('disabled', 'disabled');
        $("#ObsvCotisation").attr('disabled', 'disabled');
        $("#ObsvEngagement").attr('disabled', 'disabled');
        $("#ObsvMntRef").attr('disabled', 'disabled');
        $("#ObsvRefGest").attr('disabled', 'disabled');
}
