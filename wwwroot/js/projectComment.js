// 
function loadComments(projectId) {
  $.ajax({
    url: '/ProjectManagement/ProjectComment/GetComments?projectId=' + projectId,
    method: 'GET',
    success: function(data) {
      let commentsHtml = '';   // Convert the 2 "var" to "let" (in case of debugging)
      for (let i = 0; i < data.length; i++) {
        commentsHtml += '<div class="comments">';   // Custom CSS (optional)
        commentsHtml += '<p>' + data[i].content + '</p>';   // TROUBLESHOOT "undefined" ERROR!!!
        // commentsHtml += '<p>' + data[i].Content + '</p>';   // TROUBLESHOOT "undefined" ERROR!!!
        commentsHtml += '<span>Posted on: ' + new Date(data[i].datePosted).toLocaleDateString() + '</span>';   // TROUBLESHOOT "Posted onInvalid Date" ERROR!!!
        // commentsHtml += '<span>Posted on' + new Date(data[i].DatePosted).toLocaleDateString() + '</span>';   // TROUBLESHOOT "Posted onInvalid Date" ERROR!!!
        commentsHtml += '</div>';
      }
      $('#comment-list').html(commentsHtml);
    } 
  });  
}

$(document).ready(function() {
  
  // loadComments - Call GetComments
  const projectId = $('#majestic-view-project-comment input[name="ProjectId"]').val();
  // const projectId = $('#project-comments input[name="ProjectId"]').val();
  loadComments(projectId);
  
  // Submit event for new comment (AddComment)
  $('#add-comment-form').submit(function(evt) {
    
    // Stop default form submission
    evt.preventDefault();

    const formData = {
      ProjectId: projectId,
      Content: $('#majestic-view-project-comment textarea[name="Content"]').val()
      // Content: $('#project-comments textarea[name="Content"]').val()
    };

    $.ajax({
      url: '/ProjectManagement/ProjectComment/AddComment',
      method: 'POST',
      contentType: 'application/json',
      data: JSON.stringify(formData),
      success: function(response) {
        if (response.success) {
          $('#majestic-view-project-comment textarea[name="Content"]').val('')   // Clear new comment from form textarea
          // $('#project-comments textarea[name="Content"]').val('')   // Clear new comment from form textarea
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