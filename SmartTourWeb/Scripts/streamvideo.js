function openVideo(id)
{ currentObj = document.getElementById(id); if (1) { currentObj.innerHTML = currentObj.innerHTML; currentObj.style.display = (currentObj.style.display == 'none') ? '' : 'none'; btnObj = document.getElementById('xem_video_' + id); btnObj.innerHTML = (btnObj.innerHTML == 'Video giới thiệu') ? 'Đóng video' : 'Video giới thiệu'; } else { currentObj.style.display = ''; } }

