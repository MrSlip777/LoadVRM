# 0 準備
本スクリプトはUnity上で動作します。<br>
また、UniVRM,DynamicBoneが必要となるのでインポートしてください。<br>

[動作確認済みの環境]<br>
・Unity 2019 2.0f1<br>
・UniVRM 0.53<br>
・DynamicBone 1.2.0<br>

## 0.1 導入
1．Unityを起動する<br>
2．ReplaceVRMSpringBoneToDynamicBone.unitypackageをダブルクリックして実行する<br>
3．Unity上でインポートウインドウが表示されるのでインポートを押下する<br>

## 0.2 メニュー一覧
メニューバーにてUniVRM-(バージョン名)を開くと以下のメニューが表示されます。<br>

・DynamicBone<br>
　・ApplySetting<br>
　・ExportSetting<br>
　・Remove<br>

・VRMSpringBone<br>
　・ApplySetting<br>
　・ExportSetting<br>
　・Remove<br>

・ReplaceBone<br>
　・VRMSpringBoneToDynamicBone<br>
　・DynamicBoneToVRMSpringBone<br>

# 1 VRMSpringBone設定のファイル出力と設定反映

## 1.1 設定をassetファイル出力する
1.HierarchyにVRMSpringBone、Colliderの設定を出力したいモデルを配置します<br>
例）VRoidStudioで作成し、Blenderで編集前のVRMファイル<br>
注意）モデルはシーン上に1体だけおいてください。<br>
2.メニューにて、UniVRM-(バージョン名)→VRMSpringBone→ExportSettingを選択します<br>
3.exportボタンを押します<br>
4.Colliderフォルダ、SpringBoneフォルダに設定値が出力された.assetファイルが作成されていればOKです。<br>

## 1.2 assetファイル出力した設定を反映する
1.Colliderフォルダ、SpringBoneフォルダにVRMSpringBone、Colliderの設定ファイルがあることを確認します。<br>
2.HierarchyにVRMSpringBone、Colliderの設定を反映したいモデルを配置します<br>
注意1）fbxファイルは一度、UniVRM-(バージョン名)→ExportHumanoidを実行してVRMファイル化してください。<br>
注意2）モデルはシーン上に1体だけおいてください。<br>

3.メニューにて、UniVRM-(バージョン名)→VRMSpringBone→ApplySettingを選択します。<br>
4.Applyボタンを押します。<br>
5.モデル上のsecondaryにVRMSpringBone、各ボーンにVRMSpringBoneColliderが反映されていればOKです。<br>

## 1.3 モデルに付けられているボーンとコライダーを削除する
1.HierarchyにVRMSpringBone、Colliderの設定を削除するモデルを配置します<br>
注意1）モデルはシーン上に1体だけおいてください。<br>
2.メニューにて、UniVRM-(バージョン名)→VRMSpringBone→Removeを選択します。<br>
3.Removeボタンを押します。<br>
注意2）ボーンはsecondaryオブジェクトに付けられたモノのみ削除します。<br>
注意3）コライダーはボーンと紐付くオブジェクトに付けられたモノのみ削除します。<br>

# 2 DynamicBone設定のファイル出力と設定反映

## 2.1 設定をassetファイル出力する
1.HierarchyにDynamicBone、Colliderの設定を出力したいモデルを配置します<br>
注意1）DynamicBoneスクリプトはsecondaryオブジェクト以外からは読み出されません。<br>
注意2）モデルはシーン上に1体だけおいてください。<br>
2.メニューにて、UniVRM-(バージョン名)→DynamicBone→ExportSettingを選択します<br>
3.exportボタンを押します<br>
4.Colliderフォルダ、DynamicBoneフォルダに設定値が出力された.assetファイルが作成されていればOKです。<br>

## 2.2 assetファイル出力した設定を反映する
1.Colliderフォルダ、DynamicBoneフォルダにDynamicBone、Colliderの設定ファイルがあることを確認します。<br>
2.HierarchyにDynamicBone、Colliderの設定を反映したいモデルを配置します<br>
注意1）fbxファイルは一度、UniVRM-(バージョン名)→ExportHumanoidを実行してVRMファイル化してください。<br>
注意2）モデルはシーン上に1体だけおいてください。<br>

3.メニューにて、UniVRM-(バージョン名)→DynamicBone→ApplySettingを選択します。<br>
4.Applyボタンを押します。<br>
5.モデル上のsecondaryにVRMSpringBone、各ボーンにVRMSpringBoneColliderが反映されていればOKです。<br>

## 2.3 モデルに付けられているボーンとコライダーを削除する
1.HierarchyにDynamicBone、Colliderの設定を削除するモデルを配置します<br>
注意1）モデルはシーン上に1体だけおいてください。<br>
2.メニューにて、UniVRM-(バージョン名)→DynamicBone→Removeを選択します。<br>
3.Removeボタンを押します。<br>
注意2）ボーンはsecondaryオブジェクトに付けられたモノのみ削除します。<br>
注意3）コライダーはボーンと紐付くオブジェクトに付けられたモノのみ削除します。<br>

# 1 VRMSpringBoneをDynamicBoneに置換する方法
大まかな設定反映になります。細かい設定はDynamicBoneの各パラメータをいじってください。<br>

## 1.2 DyanamicBoneにVRMSpringBoneの設定を反映、置換する
1.Colliderフォルダ、SpringBoneフォルダにVRMSpringBone、Colliderの設定ファイルがあることを確認します。<br>
2.HierarchyにVRMSpringBone、Colliderの設定を反映したいモデルを配置します<br>
注意1）fbxファイルは一度、UniVRM-(バージョン名)→ExportHumanoidを実行してVRMファイル化してください<br>
注意2）モデルはシーン上に1体だけおいてください。<br>

3.メニューにて、UniVRM-(バージョン名)→ReplaceBone→SpringBoneToDynamicBoneを選択します<br>
4.Applyボタンを押します<br>
5.モデル上のsecondaryにDynamicBone、各ボーンにDynamicBoneColliderが反映されていればOKです。<br>

# 2 DynamicBoneをVRMSpringBoneに置換する方法
大まかな設定反映になります。細かい設定はVRMSpringBoneの各パラメータをいじってください。<br>

## 2.1 VRMSpringBoneにDyanamicBoneの設定を反映、置換する
1.Colliderフォルダ、DynamicBoneフォルダにDynamicBone、Colliderの設定ファイルがあることを確認します。<br>
2.HierarchyにDynamicBone、Colliderの設定を反映したいモデルを配置します<br>
注意1）fbxファイルは一度、UniVRM-(バージョン名)→ExportHumanoidを実行してVRMファイル化してください<br>
注意2）モデルはシーン上に1体だけおいてください。<br>

3.メニューにて、UniVRM-(バージョン名)→ReplaceBone→DynamicBoneToSpringBoneを選択します<br>
4.Applyボタンを押します<br>
5.モデル上のsecondaryにVRMSpringBone、各ボーンにVRMSpringBoneColliderが反映されていればOKです。<br>

