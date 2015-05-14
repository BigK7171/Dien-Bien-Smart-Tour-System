function limitDescription(id)
{
    var max_length = 360;
    var des_length = document.getElementById(id).innerHTML.length;
    var des = document.getElementById(id).innerHTML;
    if (des_length > max_length) {
        document.getElementById(id).innerHTML = des.substring(0, max_length) + "...";
    }
}