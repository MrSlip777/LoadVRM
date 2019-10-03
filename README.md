フォルダ：TransferVRMSpringBone<br>
内容：VRMSpringBoneの設定出力と設定反映するスクリプト　が含まれています。<br>

# VRMSpringBoneをDynamicBoneに置換する方法
	<font color="Red">大まかな設定反映になります。細かい設定はDynamicBoneの各パラメータをいじってください（沼）</font><br>

## VRMSpringBoneの設定を出力する手順
1.HierarchyにVRMSpringBone、Colliderの設定を出力したいモデルを配置します<br>
例）VRoidStudioで作成し、Blenderで編集前のVRMファイル<br>
注意）モデルはシーン上に1体だけおいてください。<br>
2.メニューにて、UniVRM-(バージョン名)→ExportVRMSpringBoneSettingを選択します<br>
3.exportボタンを押します<br>
4.Colliderフォルダ、SpringBoneフォルダに設定値が出力された.assetファイルが作成されていればOKです。<br>

## DyanamicBoneにVRMSpringBoneの設定を反映、置換する手順
1.Colliderフォルダ、SpringBoneフォルダにVRMSpringBone、Colliderの設定ファイルがあることを確認します。<br>
2.HierarchyにVRMSpringBone、Colliderの設定を反映したいモデルを配置します<br>
注意1）fbxファイルは一度、UniVRM-(バージョン名)→ExportHumanoidを実行してVRMファイル化してください<br>
注意2）モデルはシーン上に1体だけおいてください。<br>

3.メニューにて、UniVRM-(バージョン名)→ReflectSettingSpringBoneToDynamicBoneを選択します<br>
4.Applyボタンを押します<br>
5.モデル上のsecondaryにDynamicBone、各ボーンにDynamicBoneColliderが反映されていればOKです。<br>


