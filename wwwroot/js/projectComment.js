// 
function loadComments(projectId) {
  $.ajax({
    url: '/ProjectManagement/ProjectComment/GetComments?projectId=' + projectId,
    method: 'GET',
    success: function(data) {      
      let commentsHtml = '<table id="project-comment"><tr><th>Comment</th><th>Date</th></tr>';  
      
      for (let i = 0; i < data.length; i++) {
        commentsHtml += '<tr>';
        commentsHtml += '<td class="comment">' + data[i].content + '</td>';
        commentsHtml += '<td class="date">' + new Date(data[i].datePosted).toUTCString() + '</td>';
        commentsHtml += '</tr>';
      }
      commentsHtml += '</table>';
      $('#majestic-project-comments').html(commentsHtml);
    }
  });  
}

$(document).ready(function() {
  
  // loadComments - Call GetComments
  const projectId = $('#add-comment-form input[name="ProjectId"]').val();
  loadComments(projectId);
  
  // Submit event for new comment (AddComment)
  $('#add-comment-form').submit(function(evt) {
    
    // Stop default form submission
    evt.preventDefault();

    const formData = {
      ProjectId: projectId,
      Content: $('#majestic-view-project-comment textarea[name="Content"]').val()
    };

    $.ajax({
      url: '/ProjectManagement/ProjectComment/AddComment',
      method: 'POST',
      contentType: 'application/json',
      data: JSON.stringify(formData),
      success: function(response) {
        if (response.success) {
          // Clear new comment from form textarea
          $('#majestic-view-project-comment textarea[name="Content"]').val('')
          loadComments(projectId);   // Reload comments after adding a new one
        } else {
          alert(response.message);
        }
      },
      error: function(xhr, status, error) {
        alert("Error: " + xhr.responseText);
      }
    });   
    
  });  
  
});