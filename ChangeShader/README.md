# Change_MToonToUTS2.unitypackage
## 概要
MToonシェーダー → UTS2シェーダーへ置換するUnityエディタ拡張スクリプト<br>

# 0 準備
本スクリプトはUnity上で動作します。<br>
また、UniVRM,DynamicBoneが必要となるのでインポートしてください。<br>

[動作確認済みの環境]<br>
・Unity 2019 4.0f1<br>
・UniVRM 0.56.3<br>

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
