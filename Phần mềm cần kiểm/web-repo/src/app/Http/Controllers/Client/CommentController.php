<?php

namespace App\Http\Controllers\Client;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Comment;
use App\Models\Product;

class CommentController extends Controller
{
    public function getComment(Request $request, $id){
        $user = Auth()->user()->id;
        $comment = new Comment();
        $comment->product_id = $id;
        $comment->user_id = $user;
        $comment->content = $request->content;
        $comment->save();
        return back()->with('msg','successfully');
    }
}
