function uploadFile(event) {
  event.preventDefault();
  var fileInput = document.getElementById('fileInput');
  var formData = new FormData();
  formData.append('file', fileInput.files[0]);
  fetch('/upload', {
      method: 'POST',
      body: formData
  })
  .then(response => {
      if (!response.ok) {
          throw new Error('Network response was not ok');
      }
      return response.text();
  })
  .then(data => {
      alert(data);
  })
  .catch((error) => {
      console.error('Error:', error);
      alert('Error: ' + error);
  });
}