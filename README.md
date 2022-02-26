# typing

タイピングゲーム  
typing game of desktop  

https://user-images.githubusercontent.com/79097169/152627479-3dae6fad-f103-4c76-b72e-4f8efd43e253.mp4



中心的な部分を開発している途中です。コードの更新時に、新しい方と古い方の間で互換性がある保障はできません。 - 2022年1月23日  
now, developing the central part. so when the code update, I can't guarantee compatibility between new and old. - 2022 1/23  

## Now
JISキーボードに対応しています  
キーボードのデータをファイルから読めるようになりました
 
supporting JIS keyboard  
supported to read file data of keyboard

## loading files

| file type | filename extension |
| -- | -- |
| keyboard | .ntkd |  
| questions file | .ntd |  

拡張子が「.ntd」のファイルだけアプリのメニューに表示されます  
キーボードのファイルの拡張子は「.ntkd」ということにしましたが、今はほかの拡張子でも使えます  

display only the file about filename extension is ".ntd" in menu of this app  
i decided filename extension of keyboard is ".ntkd" but, you can use other filename extension now  

## plans
私がJISキーボード以外のキーボードを理解できたら、そのキーボードも対応させたい (USキーボードに対応させたい)  
私が他の言語を理解できたら、その言語も対応させたい  

if I can understand keyboard other than JIS keyboard, it will support the keyboard (I want to support US keyboard)  
if I can understand other lang, it will support the lang  

## How to use 
coming soon

### type of questions
  
#### \[general typing] - Type the displayed strings  
  
(1) ja_word  
 　日本語の単語 - Japanese words  
(2) ja_sentence  
　日本語の文章 - Japanese sentences  
(3) en_word  
　英語の単語 - English words  
(4) en_sentence  
　英語の文章 - English sentences  
  
#### \[questions typing] - Type the answer to the question  
  
(1) question_word  
 　単語の問題 - Questions of words  
(2) question_sentence  
 　文章題 - Questions of sentences  
  
#### \[math typing] - Type the calculate result of displayed numerical formula  
  
(1) dec-dec_math-addition  
 　10進数2つの足し算 - Addition of 2 decimal numbers  
(2) dec-dec_math-subtraction  
　10進数2つの引き算 - Subtraction of 2 decimal numbers  
(3) dec-dec_math-multiplication  
　10進数2つの掛け算 - Multiplication of 2 decimal numbers   
(3) dec-dec_math-division  
　10進数2つの割り算 - Division of 2 decimal numbers   
