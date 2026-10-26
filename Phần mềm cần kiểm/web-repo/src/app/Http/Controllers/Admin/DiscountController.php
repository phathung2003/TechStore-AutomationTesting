<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Discount;
use App\Models\Rank;

class DiscountController extends Controller
{
    public function getList(){
        $title = "List of discount";
        $list = Discount::all();
        if(!empty($list)){
            return view('admin.discount.list',compact('title','list'));
        }
        return view('admin.discount.list',compact('title'));      
    }
    public function getAdd(){
        $title = "Add discount";
        $rank = Rank::all();
        if(!empty($rank)){
            return view('admin.discount.add',compact('title','rank'));
        }
        return view('admin.discount.add',compact('title'));
    }
    public function postAdd(Request $request){
        $request->validate([
            'name'=>'required',
            'price' => 'required',
            'rank' => 'required',
            'amount' => 'required | integer | min:20',
        ]);
        $discount = new Discount;
        $discount->name = $request->name;
        $discount->code = $request->code;
        $discount->price = $request->price;
        $discount->rank_id = $request->rank;
        $discount->amount = $request->amount;
        $discount->date_start = $request->date_start;
        $discount->date_end = $request->date_end;
        $check = $discount->save();
        if($check){
            return redirect()->route('admin.discount.getList',compact('check'));
        }
        return redirect()->route('admin.discount.getList')->with('msg','Error');
    }
    public function getUpdate($id=0){
        if(!empty($id) && ctype_digit($id)){
        $discount = Discount::find($id);
        $rank = Rank::all();
        if(!empty($discount))
        {
            return view('admin.discount.update',compact('discount','rank'));
        }
    }
        return view('admin.discount.update');
    }
    public function postUpdate(Request $request, $id=0){
        if(!empty($id) && ctype_digit($id)){
            $request->validate([
                'name'=>'required',
                'code'=>'required',
                'price' => 'required',
                'amount' => 'required | integer | min:20',
                'date_start' => 'required',
                'date_end' => 'required'
            ]);
            $data = Discount::find($id);
            if(!empty($data)){
                $data->name =$request->name;
                $data->code =$request->code;
                $data->price = $request->price;
                $data->rank_id = $request->rank;
                $data->amount = $request->amount;
                $data->date_start = $request->date_start;
                $data->date_end = $request->date_end;
                $check = $data->save();
                if(!empty($check)){
                    return redirect()->route('admin.discount.getList',compact('check'));
                }
            }
        }
        return redirect()->route('admin.discount.getList')->with('msg','Error');
    }
    public function deleteItem($id=0){
        if(!empty($id) && ctype_digit($id)){
            $discount = Discount::find($id);
            if(!empty($discount)){
                $discount = $discount->deleteItem($id);
                return redirect()->route('admin.discount.getList')->with('msg','Success');
            }
        }
        return redirect()->route('admin.discount.getList')->with('msg','Error');
    }
}

