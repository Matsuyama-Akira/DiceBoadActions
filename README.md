![1_TitleLogo2_DBA](https://github.com/Matsuyama220061/DiceBoadActions/assets/122959720/bbddc791-e502-480c-a1e8-0227e7e5ccdb)
### Version 1.11

## 動作及び開発環境
Mac Windows11で動作を確認  
Unity 2021.3.16f1 にて開発

# ゲーム内容
サイコロを振ってマスを進み、遭遇した敵を倒していく3Dアクションゲームとなります。  
道中には、宝箱マス、回復マス、敵マスの三種類が存在し、そのマスに止まることが出来れば効果を受けられるようになっています。ただし、最後のマスに存在するボス敵マスのみ、そのマスを踏んだ時点でその敵と戦うようになっています。

## ゲームの進行
### ***タイトル画面***
<img width="700" alt="1_Title_DBA" src="https://github.com/Matsuyama220061/DiceBoadActions/assets/122959720/dc2b6a76-dbb1-40ac-8939-92b5001a7b69">

### ***初期武器選択***
<img width="700" alt="2_WeponSellect_DBA" src="https://github.com/Matsuyama220061/DiceBoadActions/assets/122959720/a134f25c-a307-4164-8d3a-974fe0f23a5b">

### ***すごろく***
  - 移動シーン
<img width="700" alt="3_PlayingMove_DBA" src="https://github.com/Matsuyama220061/DiceBoadActions/assets/122959720/5ea36702-bfeb-482d-8100-6b22bfda29e1">

### ***戦闘***
  - 装備している武器が剣
<img width="700" alt="4_PlayingBow_DBA" src="https://github.com/Matsuyama220061/DiceBoadActions/assets/122959720/4a9db984-5b34-4dd3-acbe-98e5027aea44">

  - 装備している武器が弓
<img width="700" alt="4_PlayingSword_DBA" src="https://github.com/Matsuyama220061/DiceBoadActions/assets/122959720/ea0cbdd8-58a0-4f79-9e1d-ae7517c04da4">

### ***設定画面***
<img width="700" alt="6_Settings_DBA" src="https://github.com/Matsuyama220061/DiceBoadActions/assets/122959720/f5d7002b-6014-46c0-9023-4e8a92a4b82f">

## それぞれのマスの効果
### ***宝箱マス***
  - 宝箱マスは、武器の種類(剣と弓)とその武器のレベル(コモン、レア、ユニークの三段階)からランダムに抽選し、選ばれた武器と現在持っている武器の二つから選択することが出来ます。
    - 宝箱の見た目  
<img width="600" alt="3_ItemBox_DBA" src="https://github.com/Matsuyama220061/DiceBoadActions/assets/122959720/7b5c7856-f3cf-429e-b956-b80174680a1e"></img>
    - 選択画面  
<img width="600" alt="3_PlayingWeponSellect_DBA" src="https://github.com/Matsuyama220061/DiceBoadActions/assets/122959720/31ff99a5-6192-4dc3-9897-c6d0463b4089"></img>

### ***回復マス***
  - 回復マスは、敵マスで起きた戦闘によるダメージを全回復することが出来ます。  
<img width="600" alt="3_HealBox_DBA" src="https://github.com/Matsuyama220061/DiceBoadActions/assets/122959720/52f96a87-d04f-4bba-847b-72fe74d4537c"></img>

### ***敵マス***
  - 敵マスは、対応した敵と戦闘します。敵のレベルは3段階あり、レベル1は青、レベル2はブラウン、レベル3(ボス)はデビルの色になっています。それぞれのレベル毎に攻撃力と体力が調整されています。
    - レベル1(青)  
    <img width="550" alt="3_Enemy1_DBA" src="https://github.com/Matsuyama220061/DiceBoadActions/assets/122959720/d94cc42e-de2e-44fd-9234-b11f4414e588"></img>
    - レベル２(ブラウン)  
    <img width="550" alt="3_Enemy2_DBA" src="https://github.com/Matsuyama220061/DiceBoadActions/assets/122959720/2bdfa575-da21-4a92-ad41-88c4e242f4e0"></img>
    - レベル3(デビル)  
    <img width="550" alt="3_EnemyBoss_DBA" src="https://github.com/Matsuyama220061/DiceBoadActions/assets/122959720/116650e5-3313-447a-9e05-28206ed01b02"></img>

# 操作方法
デフォルトの操作設定。ゲーム中のタイトル画面からセッティング画面に移動し、変えたいキーをクリックした後使いたいキーを入力することで変更可能です。
- キャラクターコントロール
  - 前進 : W
  - 後退 : S
  - 右折 : D
  - 左折 : A
  - ジャンプ : Space
  - ダッシュ : LeftShift
- 攻撃
  - 通常攻撃 : 左クリック
- その他
  - サイコロを振る : Q

# クレジット
- 制作者
  - 松山 洸/Matsuyama Akira
  - モデルとサウンドと2Dイラスト以外全てを作成しました。

- 敵キャラクターモデル
  - MarberS Animations様
  - UnityAssetStoreより、[Baruk the Werewolf](https://assetstore.unity.com/packages/3d/characters/creatures/baruk-the-werewolf-207272)を使用しました。

- プレイヤーキャラクターモデル及び剣モデル
  - Jose (Dogzerx) Díaz様
  - UnityAssetStoreより、[Medieval Cartoon Warriors](https://assetstore.unity.com/packages/3d/characters/medieval-cartoon-warriors-90079)を使用しました。

- サイコロモデル
  - Armor and Rum様
  - UnityAssetStoreより、[Dice d6 game ready PBR](https://assetstore.unity.com/packages/3d/props/tools/dice-d6-game-ready-pbr-200151)を使用しました。

- 弓モデル
  - JustCreate様
  - UnityAssetStoreより、[Low Poly RPG Fantasy Weapons Lite](https://assetstore.unity.com/packages/3d/props/weapons/low-poly-rpg-fantasy-weapons-lite-226554)を使用しました。


