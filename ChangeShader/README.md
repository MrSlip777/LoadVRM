# ChangeShader_UTS2_MToon
## 概要
対象モデルの以下シェーダーに置換するUnityエディタ拡張スクリプト<br>
対象のシェーダーは以下２つ<br>
・MToon：VRM/MToon<br>
・UTS2：UnityChanToonShader/Toon_DoubleShadeWithFeather_Clipping<br>

# 0 準備
本スクリプトはUnity上で動作します。<br>
また、UniVRM,UTS2が必要となるのでインポートしてください。<br>

[動作確認済みの環境]<br>
・Unity 2018 4.20f1<br>
・UniVRM 0.59.0<br>
・ユニティちゃんトゥーンシェーダー2.0 (2.0.7)<br>

## 0.1 導入
1．Unityを起動する<br>
2．ChangeShader_UTS2_MToon.unitypackageをエディタ内へドラッグ＆ドロップする<br>
3．Unity上でインポートウインドウが表示されるのでインポートを押下する<br>

## 0.2 メニュー一覧
メニューバーにてUniVRM-(バージョン名)を開くと以下のメニューが表示されます。<br>

・MaterialSetting<br>
　・Change_MToonToUTS2<br>
　・Change_UTS2ToMToon<br>

# 1 シェーダー置換

1.Hierarchyにシェーダー置換したいモデルを配置します<br>
2.メニューにて、UniVRM-(バージョン名)→MaterialSetting→Change_MToonToUTS2(またはChangeMToonToUTS2)を選択します<br>
3.シーンからTargetModelへ手順1のモデルをドラッグします（モデルを設定する）<br>
4.changeボタンを押します<br>

# ChangeShader_MToonToUTS2_URP
## 概要
MToonシェーダー → UTS2シェーダーへ置換するUnityエディタ拡張スクリプト<br>

# 0 準備
本スクリプトはUnity上で動作します。<br>
また、UniVRM,UTS2が必要となるのでインポートしてください。<br>

[動作確認済みの環境]<br>
・Unity 2019 4.0f1<br>
・UniVRM 0.56.3<br>
・Universal Toon Shader for URP 2.2.0 (UTS2)<br>

## 0.1 導入
1．Unityを起動する<br>
2．Change_MToonToUTS2.unitypackageをエディタ内へドラッグ＆ドロップする<br>
3．Unity上でインポートウインドウが表示されるのでインポートを押下する<br>

## 0.2 メニュー一覧
メニューバーにてUniVRM-(バージョン名)を開くと以下のメニューが表示されます。<br>

・MaterialSetting<br>
　・Change_MToonToUTS2<br>

# 1 MToonシェーダー → UTS2シェーダーへの置換

1.Hierarchyにシェーダー置換したいモデルを配置します<br>
2.メニューにて、UniVRM-(バージョン名)→MaterialSetting→Change_MToonToUTS2を選択します<br>
3.シーンからTargetModelへ手順1のモデルをドラッグします（モデルを設定する）<br>
4.changeボタンを押します<br>
5.Projectにて、モデルに割り当てられたマテリアルを一通り選択する<br>
※各マテリアルのインスペクタを表示することで、モデルにマテリアルの設定が反映されます<br>
