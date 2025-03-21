// 
function loadComments(projectId) {
  $.ajax({
    url: 'ProjectManagement/ProjectComments/GetComments?projectId=' + projectId,
    method: 'GET',
    success: function(data) {
      var commentsHtml = '';
      for (var i = 0; i < data.length; i++) {
        commentsHtml += '<div class="comments">';   // Custom CSS (optional)
        commentsHtml += '<p>' + data[i].Content + '</p>';
        commentsHtml += '<span>Posted on' + new Date(data[i].DatePosted).toLocaleDateString() + '</span>';
        commentsHtml += '</div>';
      }
      $('#commentList').html(commentsHtml);
    } 
  });
  
}