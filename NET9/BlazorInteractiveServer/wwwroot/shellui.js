window.ShellUI = {
    focusElement: function (elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            element.focus();
        }
    },
    
    focusBody: function () {
        document.body.focus();
    },
    
    addClassToDocument: function (className) {
        document.documentElement.classList.add(className);
    },
    
    removeClassFromDocument: function (className) {
        document.documentElement.classList.remove(className);
    },
    
    toggleClassOnDocument: function (className) {
        document.documentElement.classList.toggle(className);
    },
    
    assignFilesToInput: function (inputElement, dataTransfer) {
        if (!inputElement) {
            return false;
        }
        
        try {
            // Create a new DataTransfer object
            const dt = new DataTransfer();
            
            // Get files from the drag event's DataTransfer
            let files;
            if (dataTransfer && dataTransfer.files) {
                files = dataTransfer.files;
            } else {
                return false;
            }
            
            // Add files to the new DataTransfer
            for (let i = 0; i < files.length; i++) {
                dt.items.add(files[i]);
            }
            
            // Assign the files from the DataTransfer to the input
            inputElement.files = dt.files;
            
            // Trigger change event
            const event = new Event('change', { bubbles: true });
            inputElement.dispatchEvent(event);
            
            return true;
        } catch (error) {
            console.error('Error assigning files to input:', error);
            return false;
        }
    },
    
    setupFileDrop: function (dropZoneId, inputElementId) {
        const dropZone = document.getElementById(dropZoneId);
        const inputElement = document.getElementById(inputElementId);
        
        if (!dropZone || !inputElement) {
            return false;
        }
        
        // Remove existing listener if any
        dropZone.removeEventListener('drop', handleDrop);
        
        function handleDrop(e) {
            e.preventDefault();
            e.stopPropagation();
            
            const dataTransfer = e.dataTransfer;
            if (!dataTransfer || !dataTransfer.files || dataTransfer.files.length === 0) {
                return;
            }
            
            try {
                // Create a new DataTransfer object
                const dt = new DataTransfer();
                
                // Add files from the drag event to the new DataTransfer
                const files = dataTransfer.files;
                for (let i = 0; i < files.length; i++) {
                    dt.items.add(files[i]);
                }
                
                // Assign the files from the DataTransfer to the input
                inputElement.files = dt.files;
                
                // Trigger change event
                const changeEvent = new Event('change', { bubbles: true });
                inputElement.dispatchEvent(changeEvent);
            } catch (error) {
                console.error('Error handling drop files:', error);
            }
        }
        
        // Attach the drop handler
        dropZone.addEventListener('drop', handleDrop);
        
        return true;
    },
    
    openUrl: function (url, target) {
        if (window.open) {
            window.open(url, target || '_blank');
        } else {
            console.error('window.open is not available');
        }
    }
};

