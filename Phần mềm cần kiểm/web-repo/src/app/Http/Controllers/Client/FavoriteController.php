<?php

namespace App\Http\Controllers\Client;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Favorite;
class FavoriteController extends Controller
{
    public function getList(){
        $list = Favorite::all();
        return view('client.favorite.index',compact('list'));
    }
    public function postAdd($id){
        if(!empty($id)){
            $favorite = new Favorite();            
            $favorite->user_id = Auth()->user()->id;
            $data = Favorite::where('user_id',Auth()->user()->id)->get();
            foreach($data as $item)
            {
                if($item->product_id == $id){                    
                    return response([
                        'status' => 'error'
                    ]);
                }
            }
            $favorite->product_id = $id;
            $favorite->save();
            $data = Favorite::where('user_id',Auth()->user()->id)->get();
            $data = $data->count();
            return response([
                'status' => 'success',
                'data' => $data
            ]);
        }
       
    }
    public function deleteItem($id){
        if(!empty($id) && ctype_digit($id)){
            $favorite =new Favorite;
                $check = $favorite->deleteItem($id);
                return redirect()->route('getFavorite');
        }
        return redirect()->route('getFavorite');
    }
}
