searchVisible = 0;
transparent = true;

$(document).ready(function(){
    /*  Activate the tooltips      */
    $('[rel="tooltip"]').tooltip();
      
    $('.wizard-card').bootstrapWizard({
        'tabClass': 'nav nav-pills',
        'nextSelector': '.btn-next',
        'previousSelector': '.btn-previous',
         
         onInit : function(tab, navigation, index){
            
           //check number of tabs and fill the entire row
           var $total = navigation.find('li').length;
           $width = 100/$total;
           
           $display_width = $(document).width();
           
           if($display_width < 600 && $total > 3){
               $width = 50;
           }
           
           navigation.find('li').css('width',$width + '%');
           
        },
        onNext: function(tab, navigation, index){
            if(index == 1){
                return validateFirstStep();
            } else if(index == 2){
                return validateSecondStep();
            } else if(index == 3){
                return validateThirdStep();
            } //etc. 
              
        },
        onTabClick : function(tab, navigation, index){
            // Disable the posibility to click on tabs
            return false;
        }, 
        onTabShow: function(tab, navigation, index) {
            var $total = navigation.find('li').length;
            var $current = index+1;
            
            var wizard = navigation.closest('.wizard-card');
            
            // If it's the last tab then hide the last button and show the finish instead
            if($current >= $total) {
                $(wizard).find('.btn-next').hide();
                $(wizard).find('.btn-finish').show();
            } else {
                $(wizard).find('.btn-next').show();
                $(wizard).find('.btn-finish').hide();
            }
        }
    });

    // Prepare the preview for profile picture
    $("#wizard-picture").change(function(){
        readURL(this);
    });
    
    $('[data-toggle="wizard-radio"]').click(function(){
        wizard = $(this).closest('.wizard-card');
        wizard.find('[data-toggle="wizard-radio"]').removeClass('active');
        $(this).addClass('active');
        $(wizard).find('[type="radio"]').removeAttr('checked');
        $(this).find('[type="radio"]').attr('checked','true');
    });
    
    $('[data-toggle="wizard-checkbox"]').click(function(){
        if( $(this).hasClass('active')){
            $(this).removeClass('active');
            $(this).find('[type="checkbox"]').removeAttr('checked');
        } else {
            $(this).addClass('active');
            $(this).find('[type="checkbox"]').attr('checked','true');
        }
    });
    
    $height = $(document).height();
    $('.set-full-height').css('height',$height);
	
	// $('#username').on('input',function(e){
		// validateFirstStep()
	// });
	
	$('#password').on('input',function(){
		validateFirstStep();
	});
	
	$('#mn').on('input',function(){
		validateFirstStep();
	});
	
	$('#otp').on('input',function(){
		validateFirstStep();
	});
	
//	$('#mn').on('input',function(){
		//validateFirstStep();
	//});
    
	// $('ap').click(function(e){
		// location.
	// });
	
	$('#divgotp').hide();
	
	$('#rotp').click(function(){
		$('#divgotp').show();
		validateFirstStep();
	});
		
	$( "#dob" ).datepicker({
		format:"dd/mm/yyyy",
		changeMonth: true,
		changeYear: true
	});
	
   // $('#datePicker').datepicker({
     //       format: 'mm/dd/yyyy'
       // });
//	});

	
});

function validateFirstStep(){
    
    $(".wizard-card form").validate({
		rules: {
			// firstname: "required",
			// lastname: "required",
			// email: {
				// required: true,
				// email: true
			// }
			username: {
				required: true,
				minlength: 2
			},
			password: {
				required: true,
				minlength: 5
			},
			otp: {
				required: true,
				number:true,
				maxlength:4,
				minlength: 4
			},
			licenseno: {
				required:true,
				maxlength:15,
				minlength:15,
			},
			mn: {
				required:true,
				number:true,
				maxlength:10,
				minlength:10,
			},
			terms: {
				required:true,
			},
			agree:"required",
			// confirm_password: {
				// required: true,
				// minlength: 5,
				// equalTo: "#password"
			// },
		
			// topic: {
				// required: "#newsletter:checked",
				// minlength: 2
			// },
			//agree: "required"			

		},
		messages: {
			// firstname: "Please enter your First Name",
			// lastname: "Please enter your Last Name",
			// email: "Please enter a valid email address",

			username: {
				required: "Please enter a username",
				minlength: "Your username must consist of at least 2 characters"
			},
			password: {
				required: "Please provide a password",
				minlength: "Your password must be at least 5 characters long"
			},
			otp:{
				required:"Please provide an OTP",
				maxlength:"Your OTP should not contain more than 4 number",
				minlength: "Your OTP must consist of at least 4 number"
			},
			licenseno:{
				required:"Please provide license no / Temparary no.",
				maxlength:"License no / Temparary  should contain exactly 15 character",
				minlength:"License no / Temparary  should contain exactly 15 character",
			},
			mn:{
				required:"Please enter mobile number",
				maxlength:"Mobile number should not contain more than 10 digit",
				minlength:"Mobile number must contain 10 digit",
			},
			terms:{
				required:"Please accept policy.",
			},
			agree:"Please accept our policy",
			// confirm_password: {
				// required: "Please provide a password",
				// minlength: "Your password must be at least 5 characters long",
				// equalTo: "Please enter the same password as above"
			// },
			// email: "Please enter a valid email address",
			//agree: "Please accept our policy",
			// topic: "Please select at least 2 topics"
				
		}
	}); 
	
	if(!$(".wizard-card form").valid()){
    	//form is invalid
    	return false;
	}
	
	return true;
}

function validateSecondStep(){
   
    //code here for second step
    $(".wizard-card form").validate({
		rules: {
			otp: {
				required: true,
				minlength: 4
			}
		},
		messages: {
			otp:{
			required:"Please provide an OTP",
			minlength: "Your password must be at least 5 characters long"
			}
		}
	}); 
	
	if(!$(".wizard-card form").valid()){
    	//console.log('invalid');
    	return false;
	}
	return true;
    
}

function validateThirdStep(){
    //code here for third step
    
    
}

 //Function to show image before upload

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#wizardPicturePreview').attr('src', e.target.result).fadeIn('slow');
        }
        reader.readAsDataURL(input.files[0]);
    }
}