----------------------------------------------------
 B5T-007001 Sample Code （for C#）
----------------------------------------------------

(1) Contents
  This code provides B5T-007001(HVC-P2) C# API class and sample code using that API class.
  With this sample code, you can execute "Detection Process" and "Registration Process for Face Recognition".

  The "Detection Process" can execute any functions in all 10 functions of B5T-007001,
  and outputs the result on the display.
  And this sample is available to use STB library which stabilizes the detected results by multiple frames
  and tracks detected faces and bodies.

  The "Registration Process" can execute the registration of the Face Recognition data
  on the album of B5T-007001.
  ( This code can register to only User ID = 0, Data ID = 0 on the album. )

(2) File description
  The following files exist in the Sample/ folder.

    bin/                          Output directory for building
    Properties/                   Data storage directory of project settings
    MainForm.cs                   Sample code main
    MainForm.Designer.cs          Form screen design definition file
    MainForm.resx                 Form screen XML resource file
    p2def.cs                      Numerical Value Definitions
    connector.cs                  Connector parent class
    serial_connector.cs           Serial connector class（Connector sub-class）
    hvc_p2_api.cs                 B5T-007001 C# API class with applying STB Library
    hvc_tracking_result.cs        Class storing the results after executing commands with applying STB Library
    okao_result.cs                Class storing the results after executing commands (in common)
    grayscale_image.cs            Class storing output image
    hvc_p2_wrapper.cs             B5T-007001 command wrapper class
    hvc_result.cs                 Class storing the results after executing commands without applying STB Library
    stb.cs                        STB library interface class
    STBLib.cs                     STB library wrapper class
    STBLibConst.cs                STB library const class
    STBLibEnum.cs                 STB library enum class
    STBLibStruct.cs               STB library struct class

(3) Building method for sample code
  1. The sample code is built to be operated on Windows 7.
     It can be compiled and linked with VC10 (Visual Studio 2010 C++).
  2. The exe file is generated under Sample/bin after compilation.
     And STBLib DLL file is needed on the same directory with the exe file.
    （In advance, STB.dll is stored in that directory.)

(4) Programming guidance
  1. Description of main classes
  
    Class name            Description
    -------------------------------------------------------
    SerialConnector       Serial connector class
    HVCP2Api              HVC-P2 API class with STB library
    HVCTrackingResult     Class storing detection results
    GrayscaleImage        Class storing gray scale image
    p2def                 Definitions

      Refer to the class diagram in HVC-P2_class.png for detail.

  2. Main process flow

  [Detection process]

    // Create class
    var connector = new SerialConnector();
    var hvc_p2_api = new HVCP2Api(connector, p2def.EX_FACE|p2def.EX_AGE, p2def.USE_STB_ON);
    var hvc_tracking_result = new HVCTrackingResult();
    var img = new GrayscaleImage();

    // Connect to device via serial interface
    hvc_p2_api.connect(3, 9600, 30 * 1000);

    // Set HVC-P2 parameters
    hvc_p2_api.set_camera_angle(p2def.HVC_CAM_ANGLE_0);
    hvc_p2_api.set_threshold(500, 500, 500, 500);
    hvc_p2_api.set_detection_size(30, 8192, 40, 8192, 64, 8192);
    hvc_p2_api.set_face_angle(p2def.HVC_FACE_ANGLE_YAW_30, p2def.HVC_FACE_ANGLE_ROLL_15);

    // Set STB parameters
    hvc_p2_api.set_stb_tr_retry_count(2);
    hvc_p2_api.set_stb_tr_steadiness_param(30, 30);

    hvc_p2_api.set_stb_pe_threshold_use(300);
    hvc_p2_api.set_stb_pe_angle_use(-15, 20, -30, 30);
    hvc_p2_api.set_stb_pe_complete_frame_count(5);

    hvc_p2_api.set_stb_fr_threshold_use(300);
    hvc_p2_api.set_stb_fr_angle_use(-15, 20, -30, 30);
    hvc_p2_api.set_stb_fr_complete_frame_count(5);
    hvc_p2_api.set_stb_fr_min_ratio(60);

    // Execution
    hvc_p2_api.execute(p2def.OUT_IMG_TYPE_QVGA, hvc_tracking_result, img);

    // Get detection result
    foreach (var f in hvc_tracking_result.faces)
    {
        var posx = f.pos_x;
        var posy = f.pos_y;
        var size = f.size;
        var conf = f.conf;
        if (f.age != null)
        {
            var tracking_age = (TrackingAgeResult)f.age;
            var age = tracking_age.age;
            var age_conf = tracking_age.conf;
            var tr_status = tracking_age.tracking_status;
        }
    }

    // Save image
    img.save("img.jpg");

    // Disconnect to device
    hvc_p2_api.disconnect();


  [Face Recognition]

    // Create class
    var connector = new SerialConnector();
    var hvc_p2_api = new HVCP2Api(connector, p2def.EX_FACE|p2def.EX_RECOGNITION, p2def.USE_STB_ON);
    var hvc_tracking_result = new HVCTrackingResult();
    var reg_img = new GrayscaleImage();
    var img = new GrayscaleImage();

    // Connect to device via serial interface
    hvc_p2_api.connect(3, 9600, 30 * 1000);

    // Registration
    var user_id = 0;
    var data_id = 0;
    hvc_p2_api.register_data(user_id, data_id, reg_img);

    // Save registered image
    reg_img.save("reg_img.jpg");

    // Execution
    hvc_p2_api.execute(p2def.OUT_IMG_TYPE_NONE, hvc_tracking_result, img);

    // Get recognition result
    foreach (var f in hvc_tracking_result.faces)
    {
        var posx = f.pos_x;
        var posy = f.pos_y;
        var size = f.size;
        var conf = f.conf;
        if (f.recognition != null)
        {
            var tracking_recognition = (TrackingRecognitionResult)f.recognition;
            var user_id = tracking_recognition.uid;
            var score = tracking_recognition.score;
            var tr_status = tracking_recognition.tracking_status;
        }
    }

    // Disconnect to device
    hvc_p2_api.disconnect();


[NOTES ON USAGE]
* This sample code and documentation are copyrighted property of OMRON Corporation
* This sample code does not guarantee proper operation
* This sample code is distributed in the Apache License 2.0.

----
OMRON Corporation 
Copyright 2018 OMRON Corporation, All Rights Reserved.