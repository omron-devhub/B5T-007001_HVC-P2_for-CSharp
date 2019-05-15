----------------------------------------------------
 B5T-007001 サンプルコード (C#版)
----------------------------------------------------

(1) サンプルコード内容
  本サンプルはB5T-007001(HVC-P2)のC# APIクラスとそのクラスを用いたサンプルコードを提供します。
  サンプルコードでは「検出処理」と「顔認証用データ登録」が実行できます。
  
  「検出処理」では全10機能を実行し、その結果を画面上に出力します。
  また、本サンプルは「安定化ライブラリ(STBLib)」を使用することもでき
  複数フレーム結果を用いた結果の安定化やトラッキングが可能です。
  （顔・人体に対してトラッキング、年齢・性別・認証に対して安定化を実行しています。）
  
  「顔認証用データ登録」では顔認証データをB5T-007001上のアルバムに登録することができます。
  （本サンプルではUser ID = 0, Data ID = 0のアルバムデータにのみ登録できます。）

(2) ファイル説明
  Sample/フォルダの中に下記のファイルが存在します。

    bin/                          ビルド時の出力ディレクトリ
    Properties/                   プロジェクト設定のデータ保存ディレクトリ
    MainForm.cs                   サンプルコードメイン（検出処理）
    MainForm.Designer.cs          フォーム画面 デザイン定義ファイル
    MainForm.resx                 フォーム画面 XMLリソースファイル
    p2def.cs                      定義値ファイル
    connector.cs                  Connectorクラス（親クラス）
    serial_connector.cs           SerialConnectorクラス（Connectorのサブクラス）
    hvc_p2_api.cs                 B5T-001007 C# APIクラス（結果安定化後）
    hvc_tracking_result.cs        コマンド実行結果格納クラス(結果安定化後)
    okao_result.cs                コマンド実行結果格納クラス(共通）
    grayscale_image.cs            出力画像格納クラス
    hvc_p2_wrapper.cs             B5T-007001 コマンドラッパクラス
    hvc_result.cs                 コマンド実行結果格納クラス（結果安定化なし）
    stb.cs                        STBLib インターフェースクラス
    STBLib.cs                     STBLib ラッパクラス
    STBLibConst.cs                STBLib 定義値ファイル
    STBLibEnum.cs                 STBLib 列挙型定義ファイル
    STBLibStruct.cs               STBLib 構造体定義ファイル

(3) サンプルコードのビルド方法
  1. 本サンプルコードは Windows7 上で動作するよう作成しています。
     VC10(Visual Studio 2010 C#)でコンパイルが可能です。
  2. コンパイル後は、Sample/bin/以下にexeファイルが生成されます。
     また、exeファイルと同じディレクトリに、STBLibのDLLファイルが必要です。
    （あらかじめSTB.dllは格納されています）

(4) プログラミングガイド
  1. 主なクラスの説明
  
    クラス名              説明
    -------------------------------------------------------
    SerialConnector       シリアルコネクタクラス
    HVCP2Api              HVC-P2 API クラス（STB library使用可）
    HVCTrackingResult     検出結果格納クラス
    GrayscaleImage        グレースケール画像格納クラス
    p2def                 定数クラス

      ※詳細なクラス図は同梱の「HVC-P2_class.png」をご参照下さい。

  2. メイン処理フロー

  [検出処理]

    // クラスの生成
    var connector = new SerialConnector();
    var hvc_p2_api = new HVCP2Api(connector, p2def.EX_FACE|p2def.EX_AGE, p2def.USE_STB_ON);
    var hvc_tracking_result = new HVCTrackingResult();
    var img = new GrayscaleImage();

    // デバイスへの接続
    hvc_p2_api.connect(3, 9600, 30 * 1000);

    // HVC-P2用パラメータ設定
    hvc_p2_api.set_camera_angle(p2def.HVC_CAM_ANGLE_0);
    hvc_p2_api.set_threshold(500, 500, 500, 500);
    hvc_p2_api.set_detection_size(30, 8192, 40, 8192, 64, 8192);
    hvc_p2_api.set_face_angle(p2def.HVC_FACE_ANGLE_YAW_30, p2def.HVC_FACE_ANGLE_ROLL_15);

    // STB library用パラメータ設定
    hvc_p2_api.set_stb_tr_retry_count(2);
    hvc_p2_api.set_stb_tr_steadiness_param(30, 30);

    hvc_p2_api.set_stb_pe_threshold_use(300);
    hvc_p2_api.set_stb_pe_angle_use(-15, 20, -30, 30);
    hvc_p2_api.set_stb_pe_complete_frame_count(5);

    hvc_p2_api.set_stb_fr_threshold_use(300);
    hvc_p2_api.set_stb_fr_angle_use(-15, 20, -30, 30);
    hvc_p2_api.set_stb_fr_complete_frame_count(5);
    hvc_p2_api.set_stb_fr_min_ratio(60);

    // 実行
    hvc_p2_api.execute(p2def.OUT_IMG_TYPE_QVGA, hvc_tracking_result, img);

    // 検出結果の取得
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

    // 画像保存
    img.save("img.jpg");

    // デバイスとの切断
    hvc_p2_api.disconnect();


   [顔認証]

    // クラスの生成
    var connector = new SerialConnector();
    var hvc_p2_api = new HVCP2Api(connector, p2def.EX_FACE|p2def.EX_RECOGNITION, p2def.USE_STB_ON);
    var hvc_tracking_result = new HVCTrackingResult();
    var reg_img = new GrayscaleImage();
    var img = new GrayscaleImage();

    // デバイスへの接続
    hvc_p2_api.connect(3, 9600, 30 * 1000);

    // 顔登録
    var user_id = 0;
    var data_id = 0;
    hvc_p2_api.register_data(user_id, data_id, reg_img);

    // 登録画像の保存
    reg_img.save("reg_img.jpg");

    // 実行
    hvc_p2_api.execute(p2def.OUT_IMG_TYPE_NONE, hvc_tracking_result, img);

    // 顔認証結果の取得
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

    // デバイスとの切断
    hvc_p2_api.disconnect();


[ご使用にあたって]
・本サンプルコードおよびドキュメントの著作権はオムロンに帰属します。
・本サンプルコードは動作を保証するものではありません。
・本サンプルコードは、Apache License 2.0にて提供しています。

----
オムロン株式会社
Copyright(C) 2018 OMRON Corporation, All Rights Reserved.
