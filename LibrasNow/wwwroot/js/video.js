document.onload = changeVideo();

function changeVideo()
{
    var chosenVideo = document.getElementById("videoSelectTag").value;
    var videoTag = document.getElementById("videoNovoTag");
    videoTag.src = "/Video/ShowVideo/" + chosenVideo;
}